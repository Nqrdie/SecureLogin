import { useState } from "react"
import { login } from "../../api/accountApi"

type Props = {
  onRegisterClick?: () => void
}

export default function LoginForm({ onRegisterClick }: Props) {
  const [email, setEmail] = useState("")
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
      const res = await login(email, password)
      const data = res.data

      localStorage.setItem("token", data.token)

      setMessage("Login successful!")
      setType("success")

      setTimeout(() => {
        window.location.href = "/home"
      }, 1500)

    } catch (err: any) {
      setMessage(err.response?.data?.message || "Email or password is incorrect")
      setType("error")
    }
  }

  return (
    <div className="auth-container">
        <form onSubmit={handleSubmit} className="login-form">
            <h1>Login</h1>

            {message && (
              <div className={`message ${type}`}>
                {message}
              </div>
            )}

            <input
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
            />

            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />

            <button type="submit">Login</button>

            <button
                type="button"
                onClick={onRegisterClick}
            >
                Create account
            </button>
        </form>
    </div>
  )
}