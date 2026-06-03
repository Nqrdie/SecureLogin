import { useNavigate } from 'react-router-dom'
import LoginForm from '../components/Auth/LoginForm'

export default function LoginPage() {
  const navigate = useNavigate()

  return (
    <div className="container">
      <LoginForm onRegisterClick={() => navigate('/register')} />
    </div>
  )
}
