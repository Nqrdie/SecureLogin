import { useNavigate, useLocation } from 'react-router-dom'
import VerifyEmailForm from '../components/Auth/VerifyEmailForm'

export default function VerifyEmailPage() {
  const navigate = useNavigate()
  const location = useLocation()
  const email = (location.state as { email?: string })?.email || ''

  return (
    <div className="container">
      <VerifyEmailForm 
        email={email}
        onBackClick={() => navigate('/')}
      />
    </div>
  )
}
