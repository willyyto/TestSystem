import React, {useCallback, useEffect, useMemo, useState} from 'react';
import {
    Button,
    Chip,
    Dropdown,
    DropdownItem,
    DropdownMenu,
    DropdownTrigger,
    Input,
    Pagination,
    Table,
    TableBody,
    TableCell,
    TableColumn,
    TableHeader,
    TableRow,
} from '@nextui-org/react';
import {AdminTestTableColumns} from './AdminTestTableColumns.ts';
import {capitalize} from 'utils/utils.tsx';
import apiService from 'contexts/AdminApiService.tsx';
import ViewTestModal from './ViewTestModal.tsx';
import {Test} from 'types/Interfaces.ts';

import {useNavigate} from "react-router-dom";
import {Icon} from "@iconify/react";

const INITIAL_VISIBLE_COLUMNS = ['title', 'company', 'questions', 'isActive', 'actions'];

const statusColorMap = {
    true: 'success',
    false: 'danger',
};

const AdminTestTable: React.FC = () => {
    const [filterValue, setFilterValue] = useState('');
    const [statusFilter, setStatusFilter] = useState<string>('all');
    const [selectedKeys, setSelectedKeys] = useState(new Set([]));
    const [visibleColumns, setVisibleColumns] = useState(new Set(INITIAL_VISIBLE_COLUMNS));
    const [rowsPerPage, setRowsPerPage] = useState(5);
    const [sortDescriptor, setSortDescriptor] = useState({
        column: 'title',
        direction: 'ascending',
    });
    const [page, setPage] = useState(1);
    const [testData, setTestData] = useState<Test[]>([]);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [selectedTest, setSelectedTest] = useState<Test | null>(null);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchAdminTests = async () => {
            try {
                const data = await apiService.fetchAdminTests();
                setTestData(data);
            } catch (error) {
                console.error('Error fetching tests:', error);
            }
        };

        fetchAdminTests();
    }, []);

    const handleViewTest = (test: Test) => {
        setSelectedTest(test);
        setIsModalOpen(true);
    };

    const hasSearchFilter = Boolean(filterValue);

    const headerColumns = useMemo(() => {
        if (visibleColumns === 'all') return AdminTestTableColumns;

        return AdminTestTableColumns.filter((column) => Array.from(visibleColumns).includes(column.uid));
    }, [visibleColumns]);

    const filteredItems = useMemo(() => {
        let filteredTests = [...testData];

        if (hasSearchFilter) {
            filteredTests = filteredTests.filter((test) =>
                test.title.toLowerCase().includes(filterValue.toLowerCase()),
            );
        }

        if (statusFilter !== 'all') {
            filteredTests = filteredTests.filter((test) =>
                statusFilter === 'active' ? test.isActive : !test.isActive,
            );
        }

        return filteredTests;
    }, [testData, filterValue, statusFilter]);

    const pages = Math.ceil(filteredItems.length / rowsPerPage);

    const items = useMemo(() => {
        const start = (page - 1) * rowsPerPage;
        const end = start + rowsPerPage;

        return filteredItems.slice(start, end);
    }, [page, filteredItems, rowsPerPage]);

    const sortedItems = useMemo(() => {
        return [...items].sort((a, b) => {
            const first = a[sortDescriptor.column];
            const second = b[sortDescriptor.column];
            const cmp = first < second ? -1 : first > second ? 1 : 0;

            return sortDescriptor.direction === 'descending' ? -cmp : cmp;
        });
    }, [sortDescriptor, items]);

    const renderCell = useCallback((test, columnKey) => {
        const cellValue = test[columnKey];

        switch (columnKey) {
            case 'title':
                return (
                    <div className="flex flex-col">
                        <p className="text-bold text-small capitalize">{cellValue}</p>
                    </div>
                );
            case 'isActive':
                return (
                    <Chip className="capitalize" color={statusColorMap[test.isActive]} size="sm" variant="flat">
                        {test.isActive ? 'Active' : 'Inactive'}
                    </Chip>
                );
            case 'questions':
                return (
                    <div className="flex flex-col">
                        <p className="text-bold text-small">{test.questions.length}</p>
                    </div>
                );
            case 'actions':
                return (
                    <div className="relative flex justify-end items-center gap-2">
                        <Dropdown>
                            <DropdownTrigger>
                                <Button isIconOnly size="sm" variant="light">
                                    <Icon icon="solar:menu-dots-bold" className="text-default-300 h-6 w-6 rotate-90"/>
                                </Button>
                            </DropdownTrigger>
                            <DropdownMenu>
                                <DropdownItem onPress={() => handleViewTest(test)}>View</DropdownItem>
                                <DropdownItem>Edit</DropdownItem>
                                <DropdownItem color="danger">Delete</DropdownItem>
                            </DropdownMenu>
                        </Dropdown>
                    </div>
                );
            default:
                return cellValue;
        }
    }, []);

    const onNextPage = useCallback(() => {
        if (page < pages) {
            setPage(page + 1);
        }
    }, [page, pages]);

    const onPreviousPage = useCallback(() => {
        if (page > 1) {
            setPage(page - 1);
        }
    }, [page]);

    const onRowsPerPageChange = useCallback((e) => {
        setRowsPerPage(Number(e.target.value));
        setPage(1);
    }, []);

    const onSearchChange = useCallback((value) => {
        if (value) {
            setFilterValue(value);
            setPage(1);
        } else {
            setFilterValue('');
        }
    }, []);

    const onClear = useCallback(() => {
        setFilterValue('');
        setPage(1);
    }, []);

    const topContent = useMemo(() => {
        return (
            <div className="flex flex-col gap-4">
                <div className="flex justify-between gap-3 items-end">
                    <Input
                        isClearable
                        className="w-full sm:max-w-[44%]"
                        placeholder="Search"
                        startContent={<Icon icon="solar:minimalistic-magnifer-outline"
                                            className="h-4 w-4 text-gray-500"/>}
                        value={filterValue}
                        onClear={() => onClear()}
                        onValueChange={onSearchChange}
                    />
                    <div className="flex gap-3">
                        <Dropdown>
                            <DropdownTrigger className="hidden sm:flex">
                                <Button endContent={<Icon icon="solar:alt-arrow-down-outline" className="h-3 w-3"/>}
                                        variant="flat">
                                    Status
                                </Button>
                            </DropdownTrigger>
                            <DropdownMenu
                                aria-label="Status Filter"
                                disallowEmptySelection
                                closeOnSelect={false}
                                selectionMode="single"
                                selectedKeys={new Set([statusFilter])}
                                onSelectionChange={(keys) => setStatusFilter(Array.from(keys)[0] as string)}
                            >
                                <DropdownItem key="all">All</DropdownItem>
                                <DropdownItem key="active">Active</DropdownItem>
                                <DropdownItem key="inactive">Inactive</DropdownItem>
                            </DropdownMenu>
                        </Dropdown>
                        <Dropdown>
                            <DropdownTrigger className="hidden sm:flex">
                                <Button endContent={<Icon icon="solar:alt-arrow-down-outline" className="h-3 w-3"/>}
                                        variant="flat">
                                    Columns
                                </Button>
                            </DropdownTrigger>
                            <DropdownMenu
                                disallowEmptySelection
                                aria-label="Table Columns"
                                closeOnSelect={false}
                                selectedKeys={visibleColumns}
                                selectionMode="multiple"
                                onSelectionChange={setVisibleColumns}
                            >
                                {AdminTestTableColumns.map((column) => (
                                    <DropdownItem key={column.uid} className="capitalize">
                                        {capitalize(column.name)}
                                    </DropdownItem>
                                ))}
                            </DropdownMenu>
                        </Dropdown>
                        <Button color="primary" endContent={<Icon icon="heroicons-solid:plus"
                                                                  className="h-5 w-5 text-white"/>}
                                onClick={() => navigate('/createtest')}>
                            Add New
                        </Button>
                    </div>
                </div>
                <div className="flex justify-between items-center">
                    <span className="text-default-400 text-small">Total {testData.length} tests</span>
                    <label className="flex items-center text-default-400 text-small">
                        Rows per page:
                        <select
                            className="bg-transparent outline-none text-default-400 text-small"
                            onChange={onRowsPerPageChange}
                        >
                            <option value="5">5</option>
                            <option value="10">10</option>
                            <option value="15">15</option>
                        </select>
                    </label>
                </div>
            </div>
        );
    }, [
        filterValue,
        visibleColumns,
        onRowsPerPageChange,
        testData.length,
        onSearchChange,
        onClear,
        statusFilter,
    ]);

    const bottomContent = useMemo(() => {
        return (
            <div className="py-2 px-2 flex justify-between items-center">
        <span className="w-[30%] text-small text-default-400">
          {selectedKeys === 'all'
              ? 'All items selected'
              : `${selectedKeys.size} of ${filteredItems.length} selected`}
        </span>
                <Pagination
                    isCompact
                    showControls
                    showShadow
                    color="primary"
                    page={page}
                    total={pages}
                    onChange={setPage}
                />
                <div className="hidden sm:flex w-[30%] justify-end gap-2">
                    <Button isDisabled={pages === 1} size="sm" variant="flat" onPress={onPreviousPage}>
                        Previous
                    </Button>
                    <Button isDisabled={pages === 1} size="sm" variant="flat" onPress={onNextPage}>
                        Next
                    </Button>
                </div>
            </div>
        );
    }, [selectedKeys, items.length, page, pages, hasSearchFilter]);

    return (
        <>
            <Table
                aria-label="Example table with custom cells, pagination and sorting"
                color="primary"
                isHeaderSticky
                bottomContent={bottomContent}
                bottomContentPlacement="outside"
                classNames={{
                    wrapper: 'max-h-[382px]',
                }}
                selectedKeys={selectedKeys}
                selectionMode="multiple"
                sortDescriptor={sortDescriptor}
                topContent={topContent}
                topContentPlacement="outside"
                onSelectionChange={setSelectedKeys}
                onSortChange={setSortDescriptor}
            >
                <TableHeader columns={headerColumns}>
                    {(column) => (
                        <TableColumn
                            key={column.uid}
                            align={column.uid === 'actions' ? 'center' : 'start'}
                            allowsSorting={column.sortable}
                        >
                            {column.name}
                        </TableColumn>
                    )}
                </TableHeader>
                <TableBody emptyContent={'No tests found'} items={sortedItems}>
                    {(item) => (
                        <TableRow key={item.id}>
                            {(columnKey) => <TableCell>{renderCell(item, columnKey)}</TableCell>}
                        </TableRow>
                    )}
                </TableBody>
            </Table>

            <ViewTestModal
                isOpen={isModalOpen}
                onClose={() => setIsModalOpen(false)}
                test={selectedTest}
            />
        </>
    );
};

export default AdminTestTable;
