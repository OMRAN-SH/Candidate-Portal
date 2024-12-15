import React, { useState, useEffect } from 'react';
import { fetchCandidates, downloadResume } from '../../services/api';
import '../../styles/Pagination.css';

import Pagination from '../Pagination/Pagination';

import '../../styles/CandidatesTable.css';

const CandidatesTable = () => {
  const [candidates, setCandidates] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize] = useState(10);
  const [totalCandidates, setTotalCandidates] = useState(0);

  useEffect(() => {
    const getCandidates = async () => {
      const data = await fetchCandidates(pageNumber, pageSize);
      setCandidates(data.candidates);
      setTotalCandidates(data.totalCandidates);
    };
    getCandidates();
  }, [pageNumber, pageSize]);

  const handleResumeDownload = async (id) => {
    try {
      const response = await downloadResume(id);
      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', `resume-${id}.pdf`);
      document.body.appendChild(link);
      link.click();
      link.remove();
    } catch (error) {
      console.error('Error downloading resume:', error);
    }
  };

  return (
    <div className="container my-4">
      <h1 className="text-center mb-4">Candidate List</h1>
      <table className="table table-striped table-bordered">
        <thead className="table-dark">
          <tr>
            <th>ID</th>
            <th>Full Name</th>
            <th>Date of Birth</th>
            <th>Experience</th>
            <th>Department</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {candidates.length === 0 ? (
            <tr>
              <td colSpan="6" className="text-center">
                No candidates found.
              </td>
            </tr>
          ) : (
            candidates.map((candidate) => (
              <tr key={candidate.id}>
                <td>{candidate.id}</td>
                <td>{candidate.fullName}</td>
                <td>{new Date(candidate.dateOfBirth).toLocaleDateString()}</td>
                <td>{candidate.yearsOfExperience} years</td>
                <td>{candidate.department}</td>
                <td>
                  <button
                    onClick={() => handleResumeDownload(candidate.id)}
                    className="btn btn-primary btn-sm"
                  >
                    Download Resume
                  </button>
                </td>
              </tr>
            ))
          )}
        </tbody>
      </table>
      <Pagination
        currentPage={pageNumber}
        totalItems={totalCandidates}
        pageSize={pageSize}
        onPageChange={setPageNumber}
      />
    </div>
  );
};

export default CandidatesTable;
