import { useNavigate } from 'react-router-dom'
import RegisterForm from '../components/Auth/RegisterForm'

export default function RegisterPage() {
  const navigate = useNavigate()

  return (
    <div className="container">
      <RegisterForm 
        onBackClick={() => navigate('/')}
        onVerifyClick={(email) => navigate('/verify-email', { state: { email } })}
      />
    </div>
  )
}
