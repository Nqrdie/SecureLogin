import { useState } from "react"
import { register } from "../../api/accountApi"

type Props = {
  onBackClick?: () => void
  onVerifyClick?: (email: string) => void
}

export default function RegisterForm({ onBackClick, onVerifyClick }: Props) {
  const [name, setName] = useState("")
  const [email, setLocalEmail] = useState("")
  const [password, setPassword] = useState("")
  const [message, setMessage] = useState("")
  const [type, setType] = useState<"success" | "error" | "">("")

  const handleSubmit = async (e: React.SubmitEvent) => {
    e.preventDefault()

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/

    if(!emailRegex.test(email)) {
      setMessage("Please enter a valid email address")
      setType("error")
      return
    }

    try {
      await register(name, email, password)

      setMessage("Account created! Check your email.")
      setType("success")

      setTimeout(() => {
        onVerifyClick?.(email)
      }, 1500)

    } catch (err: any) {
      setMessage(err.response?.data?.message || "Registration failed")
      setType("error")
    }
  }

  return (
    <div className="auth-container">
        <form onSubmit={handleSubmit} className="login-form">
            <h1>Register</h1>

            {message && <div className={type}>{message}</div>}

            <input
                placeholder="Name"
                value={name}
                onChange={(e) => setName(e.target.value)}
            />

            <input
                type="email"
                placeholder="Email"
                value={email}
                onChange={(e) => setLocalEmail(e.target.value)}
            />

            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />

            <button type="submit">Register</button>

            <button
                type="button"
                onClick={onBackClick}
            >
                Back to login
            </button>
        </form>
    </div>
  )
}