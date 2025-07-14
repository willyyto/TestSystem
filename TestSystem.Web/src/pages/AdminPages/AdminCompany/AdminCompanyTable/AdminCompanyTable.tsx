import React, { useCallback, useMemo, useState } from 'react';
import { useQuery } from '@tanstack/react-query';
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
    Spinner,
} from '@heroui/react';
import { Icon } from '@iconify/react';
import { format } from "date-fns";
import { useNavigate } from 'react-router-dom';
import { companyService, Company, CompanySearchParams } from 'services/CompanyService';
import ConfirmationModal from 'components/common/ConfirmationModal';
import AddCompanyModal from './AddCompanyModal';
import RowsPerPageDropdown from "components/common/RowsPerPageDropdown";
import {AdminCompanyTableColumns} from "./AdminCompanyTableColumns";

// Utility functions
const formatDate = (dateString: string) => {
    if (!dateString) return 'N/A';
    return format(new Date(dateString), 'dd/MM/yyyy');
};

const formatStorage = (used: number, limit: number) => {
    const usedGB = (used / 1024).toFixed(2);
    const limitGB = (limit / 1024).toFixed(2);
    const percentage = limit > 0 ? ((used / limit) * 100).toFixed(1) : '0';
    return `${usedGB}GB / ${limitGB}GB (${percentage}%)`;
};

const getStorageColor = (used: number, limit: number) => {
    if (limit === 0) return 'default';
    const percentage = (used / limit) * 100;
    if (percentage >= 90) return 'danger';
    if (percentage >= 70) return 'warning';
    return 'success';
};

// Helper function to get display name for sort fields
const getSortDisplayName = (column: string) => {
    const sortDisplayNames: Record<string, string> = {
        'name': 'Name',
        'contactPerson': 'Contact Person',
        'email': 'Email',
        'subscriptionTier': 'Subscription',
        'maxUsers': 'Max Users',
        'maxTests': 'Max Tests',
        'createdOn': 'Created',
        'updatedOn': 'Updated'
    };
    return sortDisplayNames[column] || column;
};

// Mock modals for company details
const ViewCompanyModal = ({ isOpen, onClose, company }: { isOpen: boolean; onClose: () => void; company: Company | null }) => {
    if (!isOpen || !company) return null;
    return (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
            <div className="bg-white p-6 rounded-lg max-w-2xl w-full mx-4 max-h-[80vh] overflow-y-auto">
                <h2 className="text-xl font-bold mb-4">Company Details</h2>
                <div className="space-y-3">
                    <div><strong>Name:</strong> {company.name}</div>
                    <div><strong>Description:</strong> {company.description || 'N/A'}</div>
                    <div><strong>Website:</strong> {company.website || 'N/A'}</div>
                    <div><strong>Contact Person:</strong> {company.contactPerson || 'N/A'}</div>
                    <div><strong>Email:</strong> {company.email || 'N/A'}</div>
                    <div><strong>Phone:</strong> {company.phone || 'N/A'}</div>
                    <div><strong>Address:</strong> {[company.address, company.city, company.state, company.country].filter(Boolean).join(', ') || 'N/A'}</div>
                    <div><strong>Subscription:</strong> {company.subscriptionTier}</div>
                    <div><strong>Max Users:</strong> {company.maxUsers}</div>
                    <div><strong>Max Tests:</strong> {company.maxTests}</div>
                    <div><strong>Storage:</strong> {formatStorage(company.storageUsedMB, company.storageLimitMB)}</div>
                    <div><strong>Created:</strong> {formatDate(company.createdOn)}</div>
                    <div><strong>Updated:</strong> {formatDate(company.updatedOn)}</div>
                </div>
                <Button onClick={onClose} className="mt-4">Close</Button>
            </div>
        </div>
    );
};

const INITIAL_VISIBLE_COLUMNS = [
    'name', 'description', 'contactPerson', 'email', 'subscriptionTier',
    'maxUsers', 'maxTests', 'storage', 'createdOn', 'isActive', 'actions'
];

const statusColorMap = {
    true: 'success',
    false: 'danger',
} as const;

const AdminCompanyTable: React.FC = () => {
    const navigate = useNavigate();
    const [filterValue, setFilterValue] = useState<string>('');
    const [statusFilter, setStatusFilter] = useState<string>('all');
    const [subscriptionFilter, setSubscriptionFilter] = useState<string>('all');
    const [selectedKeys, setSelectedKeys] = useState<Set<React.Key>>(new Set());
    const [visibleColumns, setVisibleColumns] = useState<Set<string>>(new Set(INITIAL_VISIBLE_COLUMNS));
    const [rowsPerPage, setRowsPerPage] = useState<number>(10);
    const [sortDescriptor, setSortDescriptor] = useState<{ column: string; direction: 'ascending' | 'descending' }>({
        column: 'name',
        direction: 'ascending',
    });
    const [page, setPage] = useState<number>(1);
    const [isConfirmModalOpen, setIsConfirmModalOpen] = useState<boolean>(false);
    const [isCompanyModalOpen, setIsCompanyModalOpen] = useState<boolean>(false);
    const [isViewModalOpen, setIsViewModalOpen] = useState<boolean>(false);
    const [companyToDelete, setCompanyToDelete] = useState<Company | null>(null);
    const [selectedCompany, setSelectedCompany] = useState<Company | null>(null);

    // Build filters for the API query
    const filters = useMemo(() => {
        const params: CompanySearchParams = {
            page: page,
            pageSize: rowsPerPage,
            sortBy: sortDescriptor.column,
            sortDirection: sortDescriptor.direction === 'ascending' ? 'asc' : 'desc'
        };

        if (filterValue?.trim()) {
            params.searchTerm = filterValue.trim();
        }

        if (statusFilter !== 'all') {
            params.statuses = [statusFilter === 'active' ? 'active' : 'inactive'];
        }

        if (subscriptionFilter !== 'all') {
            params.subscriptionTiers = [subscriptionFilter];
        }

        return params;
    }, [filterValue, statusFilter, subscriptionFilter, sortDescriptor, page, rowsPerPage]);

    // Fetch companies data using TanStack Query
    const { data: companiesResponse, isLoading, error, refetch } = useQuery({
        queryKey: ['admin-companies', filters],
        queryFn: async () => {
            try {
                return await companyService.getCompanies(filters);
            } catch (error) {
                console.error('Error fetching companies:', error);
                throw error;
            }
        },
        keepPreviousData: true,
        retry: 2,
        staleTime: 5 * 60 * 1000, // 5 minutes
        cacheTime: 10 * 60 * 1000, // 10 minutes
    });

    // Extract data from the API response
    const companyData = companiesResponse?.items || [];
    const totalCount = companiesResponse?.totalCount || 0;
    const totalPages = companiesResponse?.totalPages || 1;

    const handleViewCompany = useCallback((company: Company) => {
        setSelectedCompany(company);
        setIsViewModalOpen(true);
    }, []);

    const handleDeleteCompany = useCallback(async () => {
        if (!companyToDelete) return;

        try {
            await companyService.deleteCompany(companyToDelete.id);
            setIsConfirmModalOpen(false);
            setCompanyToDelete(null);
            refetch(); // Refresh the data after deletion
            // You might want to show a success toast here
        } catch (error) {
            console.error('Failed to delete company', error);
            setIsConfirmModalOpen(false);
            setCompanyToDelete(null);
            // You might want to show an error toast here
        }
    }, [companyToDelete, refetch]);

    const headerColumns = useMemo(() => {
        if (visibleColumns === 'all') return AdminCompanyTableColumns;
        return AdminCompanyTableColumns.filter((column) =>
            Array.from(visibleColumns).includes(column.uid)
        );
    }, [visibleColumns]);

    const renderCell = useCallback((company: Company, columnKey: string) => {
        const cellValue = company[columnKey as keyof Company];

        switch (columnKey) {
            case 'name':
                return (
                    <div className="flex flex-col">
                        <p className="text-sm font-semibold">{cellValue as string}</p>
                        {company.customDomain && (
                            <p className="text-xs text-gray-500">{company.customDomain}</p>
                        )}
                    </div>
                );
            case 'description':
                return (
                    <div className="flex flex-col">
                        <p className="text-sm truncate max-w-[200px]">
                            {(cellValue as string) || 'N/A'}
                        </p>
                    </div>
                );
            case 'website':
                return (
                    <div className="flex flex-col">
                        {cellValue ? (
                            <a href={cellValue as string} target="_blank" rel="noopener noreferrer"
                               className="text-sm text-blue-600 hover:underline truncate max-w-[150px]">
                                {cellValue as string}
                            </a>
                        ) : (
                            <p className="text-sm text-gray-500">N/A</p>
                        )}
                    </div>
                );
            case 'subscriptionTier':
                return (
                    <Chip
                        className="capitalize"
                        color={company.subscriptionTier === 'premium' ? 'success' :
                            company.subscriptionTier === 'professional' ? 'warning' : 'default'}
                        size="sm"
                        variant="flat"
                    >
                        {cellValue as string}
                    </Chip>
                );
            case 'storage':
                return (
                    <div className="flex flex-col">
                        <Chip
                            className="text-xs"
                            color={getStorageColor(company.storageUsedMB, company.storageLimitMB)}
                            size="sm"
                            variant="flat"
                        >
                            {formatStorage(company.storageUsedMB, company.storageLimitMB)}
                        </Chip>
                    </div>
                );
            case 'isActive':
                return (
                    <Chip
                        className="capitalize"
                        color={statusColorMap[company.isActive]}
                        size="sm"
                        variant="flat"
                    >
                        {company.isActive ? 'Active' : 'Inactive'}
                    </Chip>
                );
            case 'createdOn':
            case 'updatedOn':
                return (
                    <div className="flex flex-col">
                        <p className="text-sm">{formatDate(cellValue as string)}</p>
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
                                <DropdownItem onPress={() => handleViewCompany(company)}>
                                    View
                                </DropdownItem>
                                <DropdownItem onPress={() => navigate(`/admin/companies/${company.id}/edit`)}>
                                    Edit
                                </DropdownItem>
                                <DropdownItem onPress={() => navigate(`/admin/companies/${company.id}/analytics`)}>
                                    Analytics
                                </DropdownItem>
                                <DropdownItem
                                    color="danger"
                                    onPress={() => {
                                        setIsConfirmModalOpen(true);
                                        setCompanyToDelete(company);
                                    }}
                                >
                                    Delete
                                </DropdownItem>
                            </DropdownMenu>
                        </Dropdown>
                    </div>
                );
            default:
                return cellValue?.toString() || 'N/A';
        }
    }, [handleViewCompany, navigate]);

    const onNextPage = useCallback(() => {
        if (page < totalPages) {
            setPage(page + 1);
        }
    }, [page, totalPages]);

    const onPreviousPage = useCallback(() => {
        if (page > 1) {
            setPage(page - 1);
        }
    }, [page]);

    const onRowsPerPageChange = useCallback((newRowsPerPage: number) => {
        setRowsPerPage(newRowsPerPage);
        setPage(1);
    }, []);

    const onSearchChange = useCallback((value: string) => {
        setFilterValue(value || '');
        setPage(1);
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
                        placeholder="Search companies..."
                        startContent={<Icon icon="solar:minimalistic-magnifer-outline" className="h-4 w-4 text-gray-500"/>}
                        value={filterValue}
                        onClear={onClear}
                        onValueChange={onSearchChange}
                    />
                    <div className="flex gap-3">
                        <Dropdown>
                            <DropdownTrigger className="hidden sm:flex">
                                <Button endContent={<Icon icon="solar:alt-arrow-down-outline" className="h-3 w-3"/>} variant="flat">
                                    Status
                                </Button>
                            </DropdownTrigger>
                            <DropdownMenu
                                aria-label="Status Filter"
                                disallowEmptySelection
                                closeOnSelect={false}
                                selectionMode="single"
                                selectedKeys={new Set([statusFilter])}
                                onSelectionChange={(keys) => {
                                    const newFilter = Array.from(keys)[0] as string;
                                    setStatusFilter(newFilter);
                                    setPage(1);
                                }}
                            >
                                <DropdownItem key="all">All</DropdownItem>
                                <DropdownItem key="active">Active</DropdownItem>
                                <DropdownItem key="inactive">Inactive</DropdownItem>
                            </DropdownMenu>
                        </Dropdown>

                        <Dropdown>
                            <DropdownTrigger className="hidden sm:flex">
                                <Button endContent={<Icon icon="solar:alt-arrow-down-outline" className="h-3 w-3"/>} variant="flat">
                                    Subscription
                                </Button>
                            </DropdownTrigger>
                            <DropdownMenu
                                aria-label="Subscription Filter"
                                disallowEmptySelection
                                closeOnSelect={false}
                                selectionMode="single"
                                selectedKeys={new Set([subscriptionFilter])}
                                onSelectionChange={(keys) => {
                                    const newFilter = Array.from(keys)[0] as string;
                                    setSubscriptionFilter(newFilter);
                                    setPage(1);
                                }}
                            >
                                <DropdownItem key="all">All</DropdownItem>
                                <DropdownItem key="basic">Basic</DropdownItem>
                                <DropdownItem key="professional">Professional</DropdownItem>
                                <DropdownItem key="premium">Premium</DropdownItem>
                            </DropdownMenu>
                        </Dropdown>

                        <Dropdown>
                            <DropdownTrigger className="hidden sm:flex">
                                <Button endContent={<Icon icon="solar:alt-arrow-down-outline" className="h-3 w-3"/>} variant="flat">
                                    Sort: {getSortDisplayName(sortDescriptor.column)}
                                    {sortDescriptor.direction === 'ascending' ? ' ↑' : ' ↓'}
                                </Button>
                            </DropdownTrigger>
                            <DropdownMenu
                                aria-label="Sort Options"
                                closeOnSelect={true}
                                selectionMode="single"
                                selectedKeys={new Set([`${sortDescriptor.column}-${sortDescriptor.direction}`])}
                                onSelectionChange={(keys) => {
                                    const selectedKey = Array.from(keys)[0] as string;
                                    const [column, direction] = selectedKey.split('-');
                                    setSortDescriptor({
                                        column,
                                        direction: direction as 'ascending' | 'descending'
                                    });
                                    setPage(1);
                                }}
                            >
                                <DropdownItem key="name-ascending">Name (A-Z)</DropdownItem>
                                <DropdownItem key="name-descending">Name (Z-A)</DropdownItem>
                                <DropdownItem key="contactPerson-ascending">Contact Person (A-Z)</DropdownItem>
                                <DropdownItem key="contactPerson-descending">Contact Person (Z-A)</DropdownItem>
                                <DropdownItem key="subscriptionTier-ascending">Subscription (A-Z)</DropdownItem>
                                <DropdownItem key="subscriptionTier-descending">Subscription (Z-A)</DropdownItem>
                                <DropdownItem key="maxUsers-ascending">Max Users (Low-High)</DropdownItem>
                                <DropdownItem key="maxUsers-descending">Max Users (High-Low)</DropdownItem>
                                <DropdownItem key="maxTests-ascending">Max Tests (Low-High)</DropdownItem>
                                <DropdownItem key="maxTests-descending">Max Tests (High-Low)</DropdownItem>
                                <DropdownItem key="createdOn-ascending">Created (Oldest)</DropdownItem>
                                <DropdownItem key="createdOn-descending">Created (Newest)</DropdownItem>
                                <DropdownItem key="updatedOn-ascending">Updated (Oldest)</DropdownItem>
                                <DropdownItem key="updatedOn-descending">Updated (Newest)</DropdownItem>
                            </DropdownMenu>
                        </Dropdown>

                        <Dropdown>
                            <DropdownTrigger className="hidden sm:flex">
                                <Button endContent={<Icon icon="solar:alt-arrow-down-outline" className="h-3 w-3"/>} variant="flat">
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
                                        {column.name}
                                    </DropdownItem>
                                ))}
                            </DropdownMenu>
                        </Dropdown>

                        <Button
                            color="primary"
                            endContent={<Icon icon="heroicons-solid:plus" className="h-5 w-5 text-white"/>}
                            onClick={() => setIsCompanyModalOpen(true)}
                        >
                            Add New
                        </Button>
                    </div>
                </div>
                <div className="flex justify-between items-center">
                    <span className="text-default-400 text-sm">
                        Total {totalCount} Companies
                        {isLoading && <Spinner size="sm" className="ml-2" />}
                    </span>
                    <label className="flex items-center text-default-400 text-sm gap-2">
                        Rows per page:
                        <RowsPerPageDropdown
                            rowsPerPage={rowsPerPage}
                            onRowsPerPageChange={onRowsPerPageChange}
                        />
                    </label>
                </div>
                {error && (
                    <div className="bg-red-50 border border-red-200 rounded-md p-3">
                        <p className="text-red-800 text-sm">
                            Error loading companies: {error.message}
                        </p>
                        <Button size="sm" variant="flat" onClick={() => refetch()} className="mt-2">
                            Retry
                        </Button>
                    </div>
                )}
            </div>
        );
    }, [
        filterValue,
        statusFilter,
        subscriptionFilter,
        visibleColumns,
        totalCount,
        isLoading,
        error,
        rowsPerPage,
        sortDescriptor,
        onSearchChange,
        onClear,
        onRowsPerPageChange,
        refetch
    ]);

    const bottomContent = useMemo(() => {
        return (
            <div className="py-2 px-2 flex justify-between items-center">
                <span className="w-[30%] text-sm text-default-400">
                    {selectedKeys === 'all'
                        ? 'All items selected'
                        : `${selectedKeys.size} of ${companyData.length} selected`}
                </span>
                <Pagination
                    isCompact
                    showControls
                    showShadow
                    color="primary"
                    page={page}
                    total={totalPages}
                    onChange={setPage}
                />
                <div className="hidden sm:flex w-[30%] justify-end gap-2">
                    <Button
                        isDisabled={page <= 1}
                        size="sm"
                        variant="flat"
                        onPress={onPreviousPage}
                    >
                        Previous
                    </Button>
                    <Button
                        isDisabled={page >= totalPages}
                        size="sm"
                        variant="flat"
                        onPress={onNextPage}
                    >
                        Next
                    </Button>
                </div>
            </div>
        );
    }, [selectedKeys, companyData.length, page, totalPages, onPreviousPage, onNextPage]);

    if (isLoading && !companyData.length) {
        return (
            <div className="flex justify-center items-center h-64">
                <Spinner size="lg" />
            </div>
        );
    }

    return (
        <>
            <Table
                aria-label="Companies table with pagination and sorting"
                color="primary"
                isHeaderSticky
                bottomContent={bottomContent}
                bottomContentPlacement="outside"
                classNames={{
                    wrapper: 'max-h-[500px] max-h-full',
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
                <TableBody
                    emptyContent={
                        error ? 'Error loading companies' : 'No companies found'
                    }
                    items={companyData}
                    isLoading={isLoading}
                    loadingContent={<Spinner label="Loading..." />}
                >
                    {(item) => (
                        <TableRow key={item.id}>
                            {(columnKey) => (
                                <TableCell>
                                    {renderCell(item, columnKey as string)}
                                </TableCell>
                            )}
                        </TableRow>
                    )}
                </TableBody>
            </Table>

            <AddCompanyModal
                isOpen={isCompanyModalOpen}
                onClose={() => setIsCompanyModalOpen(false)}
                onSuccess={() => {
                    setIsCompanyModalOpen(false);
                    refetch();
                }}
            />

            <ViewCompanyModal
                isOpen={isViewModalOpen}
                onClose={() => setIsViewModalOpen(false)}
                company={selectedCompany}
            />

            <ConfirmationModal
                isOpen={isConfirmModalOpen}
                onClose={() => setIsConfirmModalOpen(false)}
                onConfirm={handleDeleteCompany}
                title="Delete Company"
                message={`Are you sure you want to delete "${companyToDelete?.name}"? This action cannot be undone.`}
            />
        </>
    );
};

export default AdminCompanyTable;