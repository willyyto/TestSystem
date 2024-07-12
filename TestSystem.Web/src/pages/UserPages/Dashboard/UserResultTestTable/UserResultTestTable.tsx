import {
    Button,
    Input,
    Pagination,
    Table,
    TableBody,
    TableCell,
    TableColumn,
    TableHeader,
    TableRow,
} from "@nextui-org/react";
import { columns } from "./UserResultTableColumns";
import { UserResultTableCells } from "./UserResultTableCells";
import { fetchUserResults } from "contexts/UserApiService";
import React, { useCallback, useEffect, useMemo, useState } from "react";
import { Result } from "types/Interfaces";
import { Icon } from "@iconify/react";
import RowsPerPageDropdown from "components/common/RowsPerPageDropdown"; 

const UserResultTestTable: React.FC = () => {
    const [results, setUserResults] = useState<Result[]>([]);
    const [filterValue, setFilterValue] = useState('');
    const [rowsPerPage, setRowsPerPage] = useState(5);
    const [sortDescriptor, setSortDescriptor] = useState({
        column: 'test.name',
        direction: 'ascending',
    });
    const [page, setPage] = useState(1);

    useEffect(() => {
        const fetchResultTestData = async () => {
            const resultsData = await fetchUserResults();
            setUserResults(resultsData);
        };
        fetchResultTestData();
    }, []);

    const hasSearchFilter = Boolean(filterValue);

    const filteredItems = useMemo(() => {
        let filteredTests = [...results];

        if (hasSearchFilter) {
            filteredTests = filteredTests.filter((result) =>
                result.test.name.toLowerCase().includes(filterValue.toLowerCase()),
            );
        }

        return filteredTests;
    }, [results, filterValue]);

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

    const onRowsPerPageChange = useCallback((value: number) => {
        setRowsPerPage(value);
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
                        startContent={<Icon icon="solar:minimalistic-magnifer-outline" className="h-4 w-4 text-gray-500" />}
                        value={filterValue}
                        onClear={() => onClear()}
                        onValueChange={onSearchChange}
                    />
                </div>
                <div className="flex justify-between items-center">
                </div>
            </div>
        );
    }, [
        filterValue,
        results.length,
        onClear,
    ]);

    const bottomContent = useMemo(() => {
        return (
            <div className=" px-2 flex justify-between items-center">
                <span className="text-default-400  w-[20%] text-small">Total {results.length} results</span>
                <Pagination
                    isCompact
                    showControls
                    showShadow
                    color="primary"
                    page={page}
                    total={pages}
                    onChange={setPage}
                />
                <div className="hidden sm:flex w-[20%] justify-end gap-2">
                    <label className="flex items-center text-default-400 text-small">
                        Rows per page:
                    </label>
                    <RowsPerPageDropdown rowsPerPage={rowsPerPage} onRowsPerPageChange={onRowsPerPageChange} />
                </div>
            </div>
        );
    }, [items.length, page, pages, hasSearchFilter]);

    return (
        <div className="w-full flex flex-col gap-4">
            <Table aria-label="Example table with custom cells"
                   topContent={topContent}
                   topContentPlacement="outside"
                   bottomContent={bottomContent}
                   bottomContentPlacement="outside"
                   onSortChange={setSortDescriptor}
                   sortDescriptor={sortDescriptor}
            >
                <TableHeader columns={columns}>
                    {(column) => (
                        <TableColumn
                            key={column.uid}
                            align={column.uid === "actions" ? "center" : "start"}
                            allowsSorting={column.sortable}
                        >
                            {column.name}
                        </TableColumn>
                    )}
                </TableHeader>
                <TableBody items={sortedItems} emptyContent={'No results found'}>
                    {(item) => (
                        <TableRow key={item.id}>
                            {(columnKey) => (
                                <TableCell key={columnKey}>
                                    {columnKey === "test" ? (
                                        <div>{item.test.name ? item.test.name : "N/A"}</div>
                                    ) : (
                                        <UserResultTableCells result={item} columnKey={columnKey} />
                                    )}
                                </TableCell>
                            )}
                        </TableRow>
                    )}
                </TableBody>
            </Table>
        </div>
    );
};

export default UserResultTestTable;
