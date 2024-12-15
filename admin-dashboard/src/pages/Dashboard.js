import React from 'react';
import CandidatesTable from '../components/CandidatesTable/CandidatesTable';
import { useNavigate } from 'react-router-dom';

const Dashboard = () => {
  const navigate = useNavigate();

  const goToStatistics = () => {
    navigate('/statistics');
  };

  return (
    <div>
      <div className="container my-4">
        <h1 className="text-center">Admin Dashboard</h1>
        <div className="d-flex justify-content-end mb-3">
          <button className="btn btn-secondary" onClick={goToStatistics}>
            View Statistics
          </button>
        </div>
        <CandidatesTable />
      </div>
    </div>
  );
};

export default Dashboard;

