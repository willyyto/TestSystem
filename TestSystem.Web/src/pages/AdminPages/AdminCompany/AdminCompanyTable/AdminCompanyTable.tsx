import React, { useCallback, useEffect, useMemo, useState } from 'react';
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
import { capitalize } from 'utils/utils';
import { useNavigate } from 'react-router-dom';
import { Icon } from '@iconify/react';
import { format } from "date-fns";
import { Company } from 'types/Interfaces.ts';
import apiService from 'contexts/AdminApiService.tsx';
import { AdminCompanyTableColumns } from './AdminCompanyTableColumns';
import ConfirmationModal from 'components/common/ConfirmationModal';
import AddCompanyModal from './AddCompanyModal'; // Import the AddCompanyModal component
import RowsPerPageDropdown from "components/common/RowsPerPageDropdown.tsx";

const INITIAL_VISIBLE_COLUMNS = ['name', 'isActive', 'actions'];

const statusColorMap = {
    true: 'success',
    false: 'danger',
};

const AdminCompanyTable: React.FC = () => {
    const [filterValue, setFilterValue] = useState<string>('');
    const [statusFilter, setStatusFilter] = useState<string>('all');
    const [selectedKeys, setSelectedKeys] = useState<Set<React.Key>>(new Set());
    const [visibleColumns, setVisibleColumns] = useState<Set<string>>(new Set(INITIAL_VISIBLE_COLUMNS));
    const [rowsPerPage, setRowsPerPage] = useState<number>(5);
    const [sortDescriptor, setSortDescriptor] = useState<{ column: string; direction: 'ascending' | 'descending' }>({
        column: 'name',
        direction: 'ascending',
    });
    const [page, setPage] = useState<number>(1);
    const [companyData, setCompanyData] = useState<Company[]>([]);
    const [isConfirmModalOpen, setIsConfirmModalOpen] = useState<boolean>(false); // State for confirm modal
    const [isCompanyModalOpen, setIsCompanyModalOpen] = useState<boolean>(false); // State for new company modal
    const [companyToDelete, setCompanyToDelete] = useState<Company | null>(null); // State for the company to delete
    const navigate = useNavigate();

    useEffect(() => {
        const fetchAdminCompanies = async () => {
            try {
                const data = await apiService.fetchAdminCompanies();
                setCompanyData(data);
            } catch (error) {
                console.error('Error fetching companies:', error);
            }
        };

        fetchAdminCompanies();
    }, []);

    const handleDeleteCompany = async () => {
        if (companyToDelete) {
            try {
                await apiService.deleteAdminCompanyById(companyToDelete.id);
                setCompanyData(companyData.filter(company => company.id !== companyToDelete.id));
                setIsConfirmModalOpen(false);
                setCompanyToDelete(null);
                alert('Company deleted successfully.');
            } catch (error) {
                setIsConfirmModalOpen(false);
                setCompanyToDelete(null);
                console.error('Failed to delete company', error);
                alert('Failed to delete company.');
            }
        }
    };

    const formatDate = (dateString: string) => {
        const date = new Date(dateString);
        return format(date, 'dd/MM/yyyy');
    };

    const hasSearchFilter = Boolean(filterValue);

    const headerColumns = useMemo(() => {
        if (visibleColumns === 'all') return AdminCompanyTableColumns;

        return AdminCompanyTableColumns.filter((column) => Array.from(visibleColumns).includes(column.uid));
    }, [visibleColumns]);

    const filteredItems = useMemo(() => {
        let filteredCompanies = [...companyData];

        if (hasSearchFilter) {
            filteredCompanies = filteredCompanies.filter((company) =>
                company.name.toLowerCase().includes(filterValue.toLowerCase()),
            );
        }

        if (statusFilter !== 'all') {
            filteredCompanies = filteredCompanies.filter((company) =>
                statusFilter === 'active' ? company.isActive : !company.isActive,
            );
        }

        return filteredCompanies;
    }, [companyData, filterValue, statusFilter]);

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

    const renderCell = useCallback((company: Company, columnKey: string) => {
        const cellValue = company[columnKey];

        switch (columnKey) {
            case 'name':
                return (
                    <div className="flex flex-col">
                        <p className="text-sm capitalize">{cellValue}</p>
                    </div>
                );
            case 'isActive':
                return (
                    <Chip className="capitalize" color={statusColorMap[company.isActive]} size="sm" variant="flat">
                        {company.isActive ? 'Active' : 'Inactive'}
                    </Chip>
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
                                <DropdownItem>Edit</DropdownItem>
                                <DropdownItem color="danger" onPress={() => { setIsConfirmModalOpen(true); setCompanyToDelete(company); }}>Delete</DropdownItem>
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

    const onRowsPerPageChange = useCallback((e: number) => {
        setRowsPerPage(e);
        setPage(1);
    }, []);

    const onSearchChange = useCallback((value: string) => {
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
                                {AdminCompanyTableColumns.map((column) => (
                                    <DropdownItem key={column.uid} className="capitalize">
                                        {capitalize(column.name)}
                                    </DropdownItem>
                                ))}
                            </DropdownMenu>
                        </Dropdown>
                        <Button color="primary" endContent={<Icon icon="heroicons-solid:plus"
                                                                  className="h-5 w-5 text-white"/>}
                                onClick={() => setIsCompanyModalOpen(true)}>
                            Add New
                        </Button>
                    </div>
                </div>
                <div className="flex justify-between items-center">
                    <span className="text-default-400 text-sm">Total {companyData.length} Companies</span>
                    <label className="flex items-center text-default-400 text-sm gap-2">
                        Rows per page:
                        <RowsPerPageDropdown rowsPerPage={rowsPerPage} onRowsPerPageChange={onRowsPerPageChange} />
                    </label>
                </div>
            </div>
        );
    }, [
        filterValue,
        visibleColumns,
        onRowsPerPageChange,
        companyData.length,
        onSearchChange,
        onClear,
        statusFilter,
    ]);

    const bottomContent = useMemo(() => {
        return (
            <div className="py-2 px-2 flex justify-between items-center">
                <span className="w-[30%] text-sm text-default-400">
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
                <TableBody emptyContent={'No companies found'} items={sortedItems}>
                    {(item) => (
                        <TableRow key={item.id}>
                            {(columnKey) => <TableCell>{renderCell(item, columnKey)}</TableCell>}
                        </TableRow>
                    )}
                </TableBody>
            </Table>

            <AddCompanyModal
                isOpen={isCompanyModalOpen}
                onClose={() => setIsCompanyModalOpen(false)}
            />

            <ConfirmationModal
                isOpen={isConfirmModalOpen}
                onClose={() => setIsConfirmModalOpen(false)}
                onConfirm={handleDeleteCompany}
            />
        </>
    );
};

export default AdminCompanyTable;
