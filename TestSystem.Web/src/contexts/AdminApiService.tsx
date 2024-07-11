import apiClient from 'contexts/ApiClient';

// Generic API request handler
const apiRequest = async (config) => {
    try {
        const response = await apiClient(config);
        return response.data;
    } catch (error) {
        // Handle error appropriately here
        throw error;
    }
};

// API methods
export const fetchAdminTests = async () => await apiRequest({url: '/admintest', method: 'GET'});

export const fetchAdminTestById = async (testId: string) => await apiRequest({
    url: `/admintest/${testId}`,
    method: 'GET'
});
export const deleteAdminTestById = async (testId: string)  => await apiRequest({
    url: `/admintest/${testId}`,
    method: 'DELETE'
});

export const fetchAdminCompanies = async () => await apiRequest({url: '/admincompany', method: 'GET'});

export const fetchAdminCompanyById = async (testId: string) => await apiRequest({
    url: `/admincompany/${testId}`,
    method: 'GET'
});
export const deleteAdminCompanyById = async (testId: string)  => await apiRequest({
    url: `/admincompany/${testId}`,
    method: 'DELETE'
});

export const addAdminCompanyById = async (company) => await apiRequest({url: '/admincompany', method: 'POST', data: company});

export const fetchUserResults = async () => await apiRequest({url: '/usertestresult', method: 'GET'});

export const fetchUserResultById = async (resultId: string) => await apiRequest({
    url: `/usertestresult/${resultId}`,
    method: 'GET'
});

export const submitTest = async (testId: string, answers: { [key: string]: string }) => await apiRequest({
    url: '/usertestsubmission/submit',
    method: 'POST',
    data: {testId, answers}
});

export const createTest = async (testData) => await apiRequest({url: '/test', method: 'POST', data: testData});



export default {
    fetchAdminTests,
    fetchAdminTestById,
    deleteAdminTestById,
    fetchAdminCompanies,
    fetchAdminCompanyById,
    deleteAdminCompanyById,
    addAdminCompanyById,
    fetchUserResults,
    fetchUserResultById,
    submitTest,
    createTest
};
