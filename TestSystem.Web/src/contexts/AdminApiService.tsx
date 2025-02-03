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

/* Admin Test */
export const fetchAdminTests = async () => await apiRequest({url: '/admintest', method: 'GET'});

export const fetchAdminTestById = async (testId: string) => await apiRequest({
    url: `/admintest/${testId}`,
    method: 'GET'
});
export const deleteAdminTestById = async (testId: string)  => await apiRequest({
    url: `/admintest/${testId}`,
    method: 'DELETE'
});

/* Admin Company */

export const fetchAdminCompanies = async () => await apiRequest({url: '/admincompany', method: 'GET'});

export const fetchAdminCompanyById = async (companyid: string) => await apiRequest({
    url: `/admincompany/${companyid}`,
    method: 'GET'
});
export const deleteAdminCompanyById = async (companyid: string) => await apiRequest({
    url: `/admincompany/${companyid}`,
    method: 'DELETE'
});

export const addAdminCompany= async (company) => await apiRequest({url: '/admincompany', method: 'POST', data: company});

/* Admin User */
export const fetchAdminUsers = async () => await apiRequest({url: '/user', method: 'GET'});

export const fetchAdminUserById = async (userid: string) => await apiRequest({
    url: `/user/${userid}`,
    method: 'GET'
});
export const deleteAdminUserById = async (userid: string) => await apiRequest({
    url: `/user/${userid}`,
    method: 'DELETE'
});

export const addAdminUser = async (user) => await apiRequest({url: '/user', method: 'POST', data: user});

/* User View */

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
    addAdminCompany,
    fetchAdminUsers,
    fetchAdminUserById,
    deleteAdminUserById,
    addAdminUser,
    fetchUserResults,
    fetchUserResultById,
    submitTest,
    createTest
};
