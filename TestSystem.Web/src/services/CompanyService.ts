import api, { ApiResponse } from '../libs/api'

// Company Types (matching your backend DTOs)
export interface Company {
    id: string
    name: string
    description?: string
    website?: string
    logoUrl?: string
    address?: string
    city?: string
    state?: string
    country?: string
    postalCode?: string
    phone?: string
    email?: string
    contactPerson?: string
    subscriptionTier: string
    subscriptionStart?: string
    subscriptionEnd?: string
    maxUsers: number
    maxTests: number
    maxQuestionsPerTest: number
    customBrandingEnabled: boolean
    advancedReportsEnabled: boolean
    apiAccessEnabled: boolean
    storageLimitMB: number
    storageUsedMB: number
    customDomain?: string
    isActive: boolean
    isArchived: boolean
    createdOn: string
    updatedOn: string
}

export interface PagedResult<T> {
    items: T[]
    totalCount: number
    page: number
    pageSize: number
    totalPages: number
}

export interface CompanySearchParams {
    searchTerm?: string
    subscriptionTiers?: string[]
    statuses?: string[]
    createdAfter?: string
    createdBefore?: string
    sortBy?: string
    sortDirection?: string
    page?: number
    pageSize?: number
}

export interface CreateCompanyRequest {
    name: string
    description?: string
    website?: string
    logoUrl?: string
    address?: string
    city?: string
    state?: string
    country?: string
    postalCode?: string
    phone?: string
    email?: string
    contactPerson?: string
    subscriptionTier?: string
    maxUsers?: number
    maxTests?: number
    maxQuestionsPerTest?: number
    customBrandingEnabled?: boolean
    advancedReportsEnabled?: boolean
    apiAccessEnabled?: boolean
    storageLimitMB?: number
    customDomain?: string
}

export interface UpdateCompanyRequest extends CreateCompanyRequest {
    isActive?: boolean
}

export interface CompanySettings {
    companyId: string
    customCss?: string
    customDomain?: string
    emailSettings?: any
    securitySettings?: any
    brandingSettings?: any
}

export const companyService = {
    // Get all companies with search/filter support
    getCompanies: async (params?: CompanySearchParams): Promise<PagedResult<Company>> => {
        const searchParams = new URLSearchParams()

        // Add pagination
        searchParams.append('page', (params?.page || 1).toString())
        searchParams.append('pageSize', (params?.pageSize || 10).toString())

        // Add search and filters
        if (params?.searchTerm) {
            searchParams.append('searchTerm', params.searchTerm)
        }

        if (params?.subscriptionTiers?.length) {
            params.subscriptionTiers.forEach(tier => searchParams.append('subscriptionTiers', tier))
        }

        if (params?.statuses?.length) {
            params.statuses.forEach(status => searchParams.append('statuses', status))
        }

        if (params?.sortBy) {
            searchParams.append('sortBy', params.sortBy)
        }

        if (params?.sortDirection) {
            searchParams.append('sortDirection', params.sortDirection)
        }

        if (params?.createdAfter) {
            searchParams.append('createdAfter', params.createdAfter)
        }

        if (params?.createdBefore) {
            searchParams.append('createdBefore', params.createdBefore)
        }

        const response = await api.get<ApiResponse<PagedResult<Company>>>(`/admin/admincompany?${searchParams.toString()}`)
        return response.data.data
    },

    // Get company by ID
    getCompany: async (id: string): Promise<Company> => {
        const response = await api.get<ApiResponse<Company>>(`/admin/admincompany/${id}`)
        return response.data.data
    },

    // Create company
    createCompany: async (data: CreateCompanyRequest): Promise<Company> => {
        const response = await api.post<ApiResponse<Company>>('/admin/admincompany', data)
        return response.data.data
    },

    // Update company
    updateCompany: async (id: string, data: UpdateCompanyRequest): Promise<Company> => {
        const response = await api.put<ApiResponse<Company>>(`/admin/admincompany/${id}`, data)
        return response.data.data
    },

    // Delete company
    deleteCompany: async (id: string): Promise<void> => {
        await api.delete(`/admin/admincompany/${id}`)
    },

    // Get company settings
    getCompanySettings: async (id: string): Promise<CompanySettings> => {
        const response = await api.get<ApiResponse<CompanySettings>>(`/admin/admincompany/${id}/settings`)
        return response.data.data
    },

    // Update company settings
    updateCompanySettings: async (id: string, settings: CompanySettings): Promise<void> => {
        await api.put(`/admin/admincompany/${id}/settings`, settings)
    },

    // Bulk operations
    bulkUpdateCompanyStatus: async (companyIds: string[], isActive: boolean): Promise<void> => {
        await api.put('/admin/admincompany/bulk-status', { companyIds, isActive })
    },
}