import React, { useEffect, useState } from 'react';
import { getStatistics } from '../services/api';
import '../styles/Statistics.css';
import { useNavigate } from 'react-router-dom';

const Statistics = () => {
  const [statistics, setStatistics] = useState({
    totalCandidates: 0,
    candidatesByDepartment: []
  });

  const navigate = useNavigate();

  useEffect(() => {
    const fetchStatistics = async () => {
      const data = await getStatistics();
      setStatistics(data);
    };
    fetchStatistics();
  }, []);

  return (
    <div className="container my-4">
      <button className="btn btn-secondary mb-3" onClick={() => navigate('/')}>
        Back to Dashboard
      </button>
      <h1 className="text-center">Dashboard Statistics</h1>
      <div className="stats-box">
        <h2>Total Candidates: {statistics.totalCandidates}</h2>
        <h3>By Department:</h3>
        <ul>
          {statistics.candidatesByDepartment && statistics.candidatesByDepartment.length > 0 ? (
            statistics.candidatesByDepartment.map((dept) => (
              <li key={dept.department}>
                {dept.department}: {dept.count}
              </li>
            ))
          ) : (
            <li>No data available</li>
          )}
        </ul>
      </div>
    </div>
  );
};

export default Statistics;
