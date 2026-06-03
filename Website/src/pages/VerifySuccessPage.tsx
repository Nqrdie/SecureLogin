import { useEffect, useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import '../App.css';

export default function VerifySuccessPage() {
  const navigate = useNavigate();
  const { search } = useLocation();
  const [status, setStatus] = useState('Processing...');

  useEffect(() => {
    const q = new URLSearchParams(search);

    if (q.get('status') === 'success') {
      setStatus('Your email has been successfully verified.');
      return;
    }

    if (q.get('error') === 'invalid') {
      setStatus('Invalid or expired verification link.');
      return;
    }

    if (q.get('error') === 'missing') {
      setStatus('Missing verification token.');
      return;
    }

    setStatus('Unknown verification state.');
  }, [search]);

  return (
    <div className="auth-container">
      <div className="login-form">
        <div className={`message ${status.includes('success') ? 'success' : 'error'}`}>
          {status}
        </div>
        <button onClick={() => navigate('/')}>
          Back to Login
        </button>
      </div>
    </div>
  );
}