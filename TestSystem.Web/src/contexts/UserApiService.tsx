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
export const fetchUserTests = async () => {
    return await apiRequest({url: '/admintest', method: 'GET'});
};
export const fetchUserTestById = async (testId: string) => {
    return await apiRequest({url: `/admintest/${testId}`, method: 'GET'});
};

export const fetchUserResults = async () => {
    return await apiRequest({url: '/usertestresult', method: 'GET'});
};

export const fetchUserResultById = async (resultId: string) => {
    return await apiRequest({url: `/usertestresult/${resultId}`, method: 'GET'});
};

export const submitTest = async (payload: any) => {
    return await apiRequest({url: '/usertestsubmission/submit', method: 'POST', data: payload });
};

export default {
    fetchUserTests,
    fetchUserTestById,
    fetchUserResults,
    fetchUserResultById,
    submitTest
};
