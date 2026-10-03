import { useState } from 'react'
import { Navigate, useNavigate } from 'react-router-dom'
import { api, getToken, getUser, saveAuth } from '../api'

function homeFor(role) {
  if (role === 'Admin') return '/admin'
  if (role === 'Organization') return '/org'
  return '/me'
}

export default function Login() {
  const nav = useNavigate()
  const existing = getUser()
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)

  if (getToken() && existing) {
    return <Navigate to={homeFor(existing.role)} replace />
  }

  async function onSubmit(e) {
    e.preventDefault()
    setError('')
    setLoading(true)
    try {
      const data = await api('/api/Auth/login', {
        method: 'POST',
        body: JSON.stringify({ username, password }),
      })
      saveAuth(data)
      nav(homeFor(data.role))
    } catch (err) {
      setError(err.message)
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="wrap">
      <div className="box" style={{ maxWidth: 400 }}>
        <h2 style={{ marginTop: 0 }}>EmployeeAPI Login</h2>
        <p className="muted">default admin: admin / Admin@123</p>
        <form onSubmit={onSubmit}>
          <div className="form-grid">
            <label htmlFor="u">Username</label>
            <input id="u" value={username} onChange={(e) => setUsername(e.target.value)} required />
            <label htmlFor="p">Password</label>
            <input id="p" type="password" value={password} onChange={(e) => setPassword(e.target.value)} required />
            <button type="submit" disabled={loading}>{loading ? '...' : 'Login'}</button>
          </div>
        </form>
        {error && <p className="err">{error}</p>}
      </div>
    </div>
  )
}
