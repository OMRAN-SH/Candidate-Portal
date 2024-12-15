import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost/api/candidates',
  headers: { 'X-ADMIN': '1' },
});

export const fetchCandidates = async (pageNumber = 1, pageSize = 10) => {
  const response = await api.get(`/list?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  return response.data;
};

export const downloadResume = async (id) => {
  const response = await api.get(`/download-resume/${id}`, {
    responseType: 'blob',
    headers: {
      'X-ADMIN': '1',
    },
  });
  return response;
};
