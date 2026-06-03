type Props = {
  email: string
  onBackClick?: () => void
}

export default function VerifyEmailForm({ email, onBackClick }: Props) {
  return (
    <div className="auth-container">
        <form className="login-form">
            <h2>Verify Email</h2>

            <p>
                We sent a verification link to:
            </p>

            <strong>{email}</strong>

            <p>
                Please check your inbox and click the link.
            </p>

            <button onClick={onBackClick}>
                Back to Login
            </button>
        </form>
    </div>
  )
}