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
export const fetchTests = async () => {
    return await apiRequest({ url: '/test', method: 'GET' });
};

export const fetchResults = async () => {
    return await apiRequest({ url: '/testresult', method: 'GET' });
};

export const fetchResultById = async (resultId: string) => {
    return await apiRequest({ url: `/testresult/${resultId}`, method: 'GET' });
};

export const fetchTestById = async (testId: string) => {
    return await apiRequest({ url: `/test/${testId}`, method: 'GET' });
};

export const submitTest = async (testId: string, answers: { [key: string]: string }) => {
    return await apiRequest({ url: '/testsubmission/submit', method: 'POST', data: { testId, answers } });
};

export const createTest = async (testData) => {
    return await apiRequest({ url: '/test', method: 'POST', data: testData });
};

export default {
    fetchTests,
    fetchResults,
    fetchResultById,
    fetchTestById,
    submitTest,
    createTest
};
