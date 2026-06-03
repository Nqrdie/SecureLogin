import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import ProtectedRoute from './components/ProtectedRoute'

import HomeLabPage from './pages/HomeLabPage'
import LoginPage from './pages/LoginPage'
import RegisterPage from './pages/RegisterPage'
import VerifyEmailPage from './pages/VerifyEmailPage'
import VerifySuccessPage from './pages/VerifySuccessPage'

function App() {
  return (
    <BrowserRouter>
      <Routes>

        <Route path="/" element={
          <ProtectedRoute>
            <HomeLabPage />
          </ProtectedRoute>
        } />

        <Route path="/home" element={
          <ProtectedRoute>
            <HomeLabPage />
          </ProtectedRoute>
        } />

        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />

        <Route path="/verify-email" element={<VerifyEmailPage />} />
        <Route path="/verify-success" element={<VerifySuccessPage />} />

        <Route path="*" element={<Navigate to="/" />} />

      </Routes>
    </BrowserRouter>
  )
}

export default App