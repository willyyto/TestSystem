import React, { useCallback, useMemo, useState } from 'react';
import { Link } from 'react-router-dom';
import {
    PagedResult,
    Test,
    TestSearchParams
} from 'services/TestsService'
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
    Modal,
    ModalContent,
    ModalHeader,
    ModalBody,
    ModalFooter,
    useDisclosure,
} from '@heroui/react';

// Import the actual testsService
import api, {ApiResponse} from 'libs/api';

// Updated testsService that matches your actual API
const testsService = {
    // Get all tests (admin/manager)
    getTests: async (params?: TestSearchParams): Promise<PagedResult<Test>> => {
        const searchParams = new URLSearchParams()
        if (params) {
            Object.entries(params).forEach(([key, value]) => {
                if (value !== undefined && value !== null) {
                    if (Array.isArray(value)) {
                        value.forEach(v => searchParams.append(key, v))
                    } else {
                        searchParams.append(key, String(value))
                    }
                }
            })
        }

        const response = await api.get<ApiResponse<PagedResult<Test>>>(`/admin/admintest?${searchParams.toString()}`)
        return response.data.data
    },

    // Get available tests for user
    getAvailableTests: async (): Promise<Test[]> => {
        const response = await api.get<ApiResponse<Test[]>>('/user/usertest/available')
        return response.data.data
    },

    // Delete test
    deleteTest: async (id: string): Promise<void> => {
        await api.delete(`/admin/admintest/${id}`)
    },

    // Get test for admin/manager
    getTestAdmin: async (id: string): Promise<Test> => {
        const response = await api.get<ApiResponse<Test>>(`/admin/admintest/${id}`)
        return response.data.data
    },

    // Bulk operations
    bulkUpdateTestStatus: async (testIds: string[], isActive: boolean): Promise<void> => {
        await api.put('/admin/admintest/bulk-status', { testIds, isActive })
    },
};

// Utility functions
const formatDate = (dateString) => {
    if (!dateString) return 'N/A';
    return new Date(dateString).toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'short',
        day: 'numeric'
    });
};

const formatDuration = (duration) => {
    if (!duration) return 'N/A';

    // Handle TimeSpan format from C# (e.g., "01:30:00" or "1.02:30:00")
    const parts = duration.split(':');
    if (parts.length >= 2) {
        const hours = parseInt(parts[0]);
        const minutes = parseInt(parts[1]);

        if (hours === 0) return `${minutes}m`;
        if (minutes === 0) return `${hours}h`;
        return `${hours}h ${minutes}m`;
    }

    return duration;
};

// Helper function to get display name for sort fields
const getSortDisplayName = (column) => {
    const sortDisplayNames = {
        'name': 'Name',
        'company': 'Company',
        'testType': 'Test Type',
        'passMark': 'Pass Mark',
        'maximumAttempts': 'Max Attempts',
        'startDate': 'Start Date',
        'endDate': 'End Date',
        'createdOn': 'Created',
        'updatedOn': 'Updated',
        'duration': 'Duration'
    };
    return sortDisplayNames[column] || column;
};

// Table columns definition that matches your backend
const AdminTestTableColumns = [
    { name: "Name", uid: "name", sortable: true },
    { name: "Company", uid: "company", sortable: true },
    { name: "Type", uid: "testType", sortable: true },
    { name: "Duration", uid: "duration", sortable: true },
    { name: "Pass Mark", uid: "passMark", sortable: true },
    { name: "Timed", uid: "isTimed", sortable: true },
    { name: "Shuffle", uid: "shuffleQuestions", sortable: true },
    { name: "Max Attempts", uid: "maximumAttempts", sortable: true },
    { name: "Questions", uid: "questions", sortable: true },
    { name: "Feedback", uid: "feedback", sortable: true },
    { name: "Access", uid: "testAccessControl", sortable: true },
    { name: "Grading", uid: "gradingScheme", sortable: true },
    { name: "Retake Policy", uid: "retakePolicy", sortable: false },
    { name: "Visibility", uid: "visibility", sortable: true },
    { name: "Start Date", uid: "startDate", sortable: true },
    { name: "End Date", uid: "endDate", sortable: true },
    { name: "Status", uid: "isActive", sortable: true },
    { name: "Actions", uid: "actions", sortable: false },
];

// HeroUI Modal Components
const ViewTestModal = ({ isOpen, onClose, test }) => {
    return (
        <Modal
            isOpen={isOpen}
            onClose={onClose}
            size="2xl"
            scrollBehavior="inside"
        >
            <ModalContent>
                <ModalHeader className="flex flex-col gap-1">
                    Test Details
                </ModalHeader>
                <ModalBody>
                    {test && (
                        <div className="space-y-4">
                            <div>
                                <p className="font-semibold">Name:</p>
                                <p >{test.name}</p>
                            </div>
                            <div>
                                <p className="font-semibold">Company:</p>
                                <p >{test.company}</p>
                            </div>
                            <div>
                                <p className="font-semibold">Description:</p>
                                <p >{test.description || 'No description available'}</p>
                            </div>
                            <div>
                                <p className="font-semibold">Test Type:</p>
                                <p >{test.testType}</p>
                            </div>
                            <div>
                                <p className="font-semibold">Duration:</p>
                                <p >{formatDuration(test.duration)}</p>
                            </div>
                            <div>
                                <p className="font-semibold">Pass Mark:</p>
                                <p >{test.passMark}%</p>
                            </div>
                            <div>
                                <p className="font-semibold">Questions:</p>
                                <p >{test.questions?.length || 0}</p>
                            </div>
                            <div>
                                <p className="font-semibold">Status:</p>
                                <Chip
                                    color={test.isActive ? 'success' : 'danger'}
                                    size="sm"
                                    variant="flat"
                                >
                                    {test.isActive ? 'Active' : 'Inactive'}
                                </Chip>
                            </div>
                        </div>
                    )}
                </ModalBody>
                <ModalFooter>
                    <Button color="primary" onPress={onClose}>
                        Close
                    </Button>
                </ModalFooter>
            </ModalContent>
        </Modal>
    );
};

const ConfirmationModal = ({ isOpen, onClose, onConfirm, title, message }) => {
    return (
        <Modal
            isOpen={isOpen}
            onClose={onClose}
            size="md"
        >
            <ModalContent>
                <ModalHeader className="flex flex-col gap-1">
                    {title}
                </ModalHeader>
                <ModalBody>
                    <p>{message}</p>
                </ModalBody>
                <ModalFooter>
                    <Button variant="light" onPress={onClose}>
                        Cancel
                    </Button>
                    <Button color="danger" onPress={onConfirm}>
                        Confirm
                    </Button>
                </ModalFooter>
            </ModalContent>
        </Modal>
    );
};

const RetakePolicyModal = ({ isOpen, onClose, retakePolicy }) => {
    return (
        <Modal
            isOpen={isOpen}
            onClose={onClose}
            size="md"
        >
            <ModalContent>
                <ModalHeader className="flex flex-col gap-1">
                    Retake Policy
                </ModalHeader>
                <ModalBody>
                    {retakePolicy ? (
                        <div className="space-y-3">
                            <div>
                                <p className="font-semibold">Allow Retakes:</p>
                                <Chip
                                    color={retakePolicy.allowRetakes ? 'success' : 'danger'}
                                    size="sm"
                                    variant="flat"
                                >
                                    {retakePolicy.allowRetakes ? 'Yes' : 'No'}
                                </Chip>
                            </div>
                            <div>
                                <p className="font-semibold">Max Retakes:</p>
                                <p >{retakePolicy.maxRetakes || 'Unlimited'}</p>
                            </div>
                            <div>
                                <p className="font-semibold">Retake Interval:</p>
                                <p >{retakePolicy.retakeInterval || 'No restriction'}</p>
                            </div>
                        </div>
                    ) : (
                        <p className="text-gray-500">No retake policy configured</p>
                    )}
                </ModalBody>
                <ModalFooter>
                    <Button color="primary" onPress={onClose}>
                        Close
                    </Button>
                </ModalFooter>
            </ModalContent>
        </Modal>
    );
};

const RowsPerPageDropdown = ({ rowsPerPage, onRowsPerPageChange }) => {
    return (
        <select
            value={rowsPerPage}
            onChange={(e) => onRowsPerPageChange(parseInt(e.target.value))}
            className="border rounded px-2 py-1 text-sm"
        >
            <option value={5}>5</option>
            <option value={10}>10</option>
            <option value={25}>25</option>
            <option value={50}>50</option>
        </select>
    );
};

const INITIAL_VISIBLE_COLUMNS = [
    'name', 'company', 'testType', 'duration', 'passMark', 'isTimed',
    'shuffleQuestions', 'maximumAttempts', 'feedback', 'testAccessControl',
    'gradingScheme', 'visibility', 'questions', 'retakePolicy', 'startDate',
    'endDate', 'isActive', 'actions'
];

const statusColorMap = {
    true: 'success',
    false: 'danger',
};

const AdminTestTable = () => {
    const [filterValue, setFilterValue] = useState('');
    const [statusFilter, setStatusFilter] = useState('all');
    const [selectedKeys, setSelectedKeys] = useState(new Set([]));
    const [visibleColumns, setVisibleColumns] = useState(new Set(INITIAL_VISIBLE_COLUMNS));
    const [rowsPerPage, setRowsPerPage] = useState(5);
    const [sortDescriptor, setSortDescriptor] = useState({
        column: 'name',
        direction: 'ascending',
    });
    const [page, setPage] = useState(1);
    const [selectedTest, setSelectedTest] = useState(null);
    const [testToDelete, setTestToDelete] = useState(null);

    // HeroUI useDisclosure hooks for modals
    const {
        isOpen: isViewModalOpen,
        onOpen: onViewModalOpen,
        onClose: onViewModalClose
    } = useDisclosure();

    const {
        isOpen: isConfirmModalOpen,
        onOpen: onConfirmModalOpen,
        onClose: onConfirmModalClose
    } = useDisclosure();

    const {
        isOpen: isRetakePolicyModalOpen,
        onOpen: onRetakePolicyModalOpen,
        onClose: onRetakePolicyModalClose
    } = useDisclosure();

    // Build filters for the API query to match your backend expectations
    const filters = useMemo(() => {
        const params: TestSearchParams = {
            page: page,
            pageSize: rowsPerPage,
            sortBy: sortDescriptor.column,
            sortDirection: sortDescriptor.direction === 'ascending' ? 'asc' : 'desc'
        };

        if (filterValue?.trim()) {
            params.searchTerm = filterValue.trim();
        }

        if (statusFilter !== 'all') {
            // Map status filter to what backend expects
            params.statuses = [statusFilter === 'active' ? 'active' : 'inactive'];
        }

        return params;
    }, [filterValue, statusFilter, sortDescriptor, page, rowsPerPage]);

    // Fetch tests data using TanStack Query with proper error handling
    const { data: testsResponse, isLoading, error, refetch } = useQuery({
        queryKey: ['admin-tests', filters],
        queryFn: async () => {
            try {
                return await testsService.getTests(filters);
            } catch (error) {
                console.error('Error fetching tests:', error);
                throw error;
            }
        },
        keepPreviousData: true,
        retry: 2,
        staleTime: 5 * 60 * 1000, // 5 minutes
        cacheTime: 10 * 60 * 1000, // 10 minutes
    });

    // Extract data from the API response structure
    const testData = testsResponse?.items || [];
    const totalCount = testsResponse?.totalCount || 0;
    const totalPages = testsResponse?.totalPages || 1;

    const handleViewTest = useCallback((test) => {
        setSelectedTest(test);
        onViewModalOpen();
    }, [onViewModalOpen]);

    const handleDeleteTest = useCallback(async () => {
        if (!testToDelete) return;

        try {
            await testsService.deleteTest(testToDelete.id);
            onConfirmModalClose();
            setTestToDelete(null);
            refetch(); // Refresh the data after deletion
            // You might want to show a success toast here
        } catch (error) {
            console.error('Failed to delete test', error);
            onConfirmModalClose();
            setTestToDelete(null);
            // You might want to show an error toast here
        }
    }, [testToDelete, refetch, onConfirmModalClose]);

    const handleViewRetakePolicy = useCallback((test) => {
        setSelectedTest(test);
        onRetakePolicyModalOpen();
    }, [onRetakePolicyModalOpen]);

    const headerColumns = useMemo(() => {
        if (visibleColumns === 'all') return AdminTestTableColumns;
        return AdminTestTableColumns.filter((column) =>
            Array.from(visibleColumns).includes(column.uid)
        );
    }, [visibleColumns]);

    const renderCell = useCallback((test, columnKey) => {
        const cellValue = test[columnKey];

        switch (columnKey) {
            case 'name':
                return (
                    <div className="flex flex-col">
                        <p className="text-sm font-semibold">{cellValue}</p>
                        {test.description && (
                            <p className="text-xs text-gray-500 truncate max-w-[200px]">
                                {test.description}
                            </p>
                        )}
                    </div>
                );
            case 'company':
                return (
                    <div className="flex flex-col">
                        <p className="text-sm">{cellValue}</p>
                    </div>
                );
            case 'isActive':
                return (
                    <Chip
                        className="capitalize"
                        color={statusColorMap[test.isActive]}
                        size="sm"
                        variant="flat"
                    >
                        {test.isActive ? 'Active' : 'Inactive'}
                    </Chip>
                );
            case 'isTimed':
                return (
                    <Chip
                        className="capitalize"
                        color={test.isTimed ? 'success' : 'default'}
                        size="sm"
                        variant="flat"
                    >
                        {test.isTimed ? 'Yes' : 'No'}
                    </Chip>
                );
            case 'shuffleQuestions':
                return (
                    <Chip
                        className="capitalize"
                        color={test.shuffleQuestions ? 'success' : 'default'}
                        size="sm"
                        variant="flat"
                    >
                        {test.shuffleQuestions ? 'Yes' : 'No'}
                    </Chip>
                );
            case 'duration':
                return (
                    <div className="flex flex-col">
                        <p className="text-sm">{formatDuration(cellValue)}</p>
                    </div>
                );
            case 'startDate':
            case 'endDate':
                return (
                    <div className="flex flex-col">
                        <p className="text-sm">{formatDate(cellValue)}</p>
                    </div>
                );
            case 'questions':
                return (
                    <div className="flex flex-col">
                        <p className="text-sm">{test.questions?.length || 0}</p>
                    </div>
                );
            case 'retakePolicy':
                return (
                    <div className="flex flex-col">
                        <Button
                            size="sm"
                            variant="ghost"
                            endContent={<span className="text-xs">→</span>}
                            onClick={() => handleViewRetakePolicy(test)}
                        >
                            View
                        </Button>
                    </div>
                );
            case 'actions':
                return (
                    <div className="relative flex justify-end items-center gap-2">
                        <Dropdown>
                            <DropdownTrigger>
                                <Button isIconOnly size="sm" variant="light">
                                    <span className="text-default-300 rotate-90">⋯</span>
                                </Button>
                            </DropdownTrigger>
                            <DropdownMenu>
                                <DropdownItem onPress={() => handleViewTest(test)}>
                                    View
                                </DropdownItem>
                                <DropdownItem onPress={() => console.log('Edit test:', test.id)}>
                                    Edit
                                </DropdownItem>
                                <DropdownItem
                                    color="danger"
                                    onPress={() => {
                                        setTestToDelete(test);
                                        onConfirmModalOpen();
                                    }}
                                >
                                    Delete
                                </DropdownItem>
                            </DropdownMenu>
                        </Dropdown>
                    </div>
                );
            default:
                return cellValue?.toString() || '';
        }
    }, [handleViewTest, handleViewRetakePolicy, onConfirmModalOpen]);

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

    const onRowsPerPageChange = useCallback((newRowsPerPage) => {
        setRowsPerPage(newRowsPerPage);
        setPage(1);
    }, []);

    const onSearchChange = useCallback((value) => {
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
                        placeholder="Search tests..."
                        startContent={<span>🔍</span>}
                        value={filterValue}
                        onClear={onClear}
                        onValueChange={onSearchChange}
                    />
                    <div className="flex gap-3">
                        <Dropdown>
                            <DropdownTrigger className="hidden sm:flex">
                                <Button
                                    endContent={<span>▼</span>}
                                    variant="flat"
                                >
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
                                    const newFilter = Array.from(keys)[0];
                                    setStatusFilter(newFilter);
                                    setPage(1);
                                }}
                            >
                                <DropdownItem key="all">All</DropdownItem>
                                <DropdownItem key="active">Active</DropdownItem>
                                <DropdownItem key="inactive">Inactive</DropdownItem>
                            </DropdownMenu>
                        </Dropdown>

                        {/* New Sort Dropdown */}
                        <Dropdown>
                            <DropdownTrigger className="hidden sm:flex">
                                <Button
                                    endContent={<span>▼</span>}
                                    variant="flat"
                                >
                                    Sort: {getSortDisplayName(sortDescriptor.column)}
                                    {sortDescriptor.direction === 'ascending' ? ' 🡅 ' : ' 🡇 '}
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
                                <DropdownItem key="company-ascending">Company (A-Z)</DropdownItem>
                                <DropdownItem key="company-descending">Company (Z-A)</DropdownItem>
                                <DropdownItem key="testType-ascending">Test Type (A-Z)</DropdownItem>
                                <DropdownItem key="testType-descending">Test Type (Z-A)</DropdownItem>
                                <DropdownItem key="passMark-ascending">Pass Mark (Low-High)</DropdownItem>
                                <DropdownItem key="passMark-descending">Pass Mark (High-Low)</DropdownItem>
                                <DropdownItem key="maximumAttempts-ascending">Max Attempts (Low-High)</DropdownItem>
                                <DropdownItem key="maximumAttempts-descending">Max Attempts (High-Low)</DropdownItem>
                                <DropdownItem key="startDate-ascending">Start Date (Oldest)</DropdownItem>
                                <DropdownItem key="startDate-descending">Start Date (Newest)</DropdownItem>
                                <DropdownItem key="endDate-ascending">End Date (Oldest)</DropdownItem>
                                <DropdownItem key="endDate-descending">End Date (Newest)</DropdownItem>
                                <DropdownItem key="createdOn-ascending">Created (Oldest)</DropdownItem>
                                <DropdownItem key="createdOn-descending">Created (Newest)</DropdownItem>
                                <DropdownItem key="updatedOn-ascending">Updated (Oldest)</DropdownItem>
                                <DropdownItem key="updatedOn-descending">Updated (Newest)</DropdownItem>
                                <DropdownItem key="duration-ascending">Duration (Shortest)</DropdownItem>
                                <DropdownItem key="duration-descending">Duration (Longest)</DropdownItem>
                            </DropdownMenu>
                        </Dropdown>

                        <Dropdown>
                            <DropdownTrigger className="hidden sm:flex">
                                <Button
                                    endContent={<span>▼</span>}
                                    variant="flat"
                                >
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
                                        {column.name}
                                    </DropdownItem>
                                ))}
                            </DropdownMenu>
                        </Dropdown>
                        <Button
                            color="primary"
                            endContent={<span>+</span>}
                            onClick={() => console.log('Navigate to create test')}
                        >
                            Add New
                        </Button>
                    </div>
                </div>
                <div className="flex justify-between items-center">
                    <span className="text-default-400 text-sm">
                        Total {totalCount} Tests
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
                            Error loading tests: {error.message}
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
                        : `${selectedKeys.size} of ${testData.length} selected`}
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
    }, [selectedKeys, testData.length, page, totalPages, onPreviousPage, onNextPage]);

    if (isLoading && !testData.length) {
        return (
            <div className="flex justify-center items-center h-64">
                <Spinner size="lg" />
            </div>
        );
    }

    return (
        <>
            <Table
                aria-label="Tests table with pagination and sorting"
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
                        error ? 'Error loading tests' : 'No tests found'
                    }
                    items={testData}
                    isLoading={isLoading}
                    loadingContent={<Spinner label="Loading..." />}
                >
                    {(item) => (
                        <TableRow key={item.id}>
                            {(columnKey) => (
                                <TableCell>
                                    {renderCell(item, columnKey)}
                                </TableCell>
                            )}
                        </TableRow>
                    )}
                </TableBody>
            </Table>

            <ViewTestModal
                isOpen={isViewModalOpen}
                onClose={onViewModalClose}
                test={selectedTest}
            />

            <ConfirmationModal
                isOpen={isConfirmModalOpen}
                onClose={onConfirmModalClose}
                onConfirm={handleDeleteTest}
                title="Delete Test"
                message={`Are you sure you want to delete "${testToDelete?.name}"? This action cannot be undone.`}
            />

            <RetakePolicyModal
                isOpen={isRetakePolicyModalOpen}
                onClose={onRetakePolicyModalClose}
                retakePolicy={selectedTest?.retakePolicy}
            />
        </>
    );
};

export default AdminTestTable;