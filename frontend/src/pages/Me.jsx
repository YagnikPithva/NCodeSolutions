import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { api, clearAuth, getUser } from '../api'

export default function Me() {
  const nav = useNavigate()
  const user = getUser()
  const [data, setData] = useState(null)
  const [error, setError] = useState('')

  function logout() {
    clearAuth()
    nav('/')
  }

  useEffect(() => {
    api('/api/me')
      .then(setData)
      .catch((err) => setError(err.message))
  }, [])

  return (
    <div className="wrap">
      <div className="topbar">
        <div>
          <strong>My profile</strong>{' '}
          <span className="muted">({user?.username} / {user?.role})</span>
        </div>
        <button type="button" onClick={logout}>Logout</button>
      </div>

      {error && <p className="err">{error}</p>}

      <div className="box">
        {!data && !error && <p className="muted">loading...</p>}
        {typeof data === 'string' && <p>{data}</p>}
        {data && typeof data === 'object' && (
          <table>
            <tbody>
              <tr><th>Id</th><td>{data.id}</td></tr>
              <tr><th>Full name</th><td>{data.fullName}</td></tr>
              <tr><th>Email</th><td>{data.email}</td></tr>
              <tr><th>Age</th><td>{data.age}</td></tr>
              <tr><th>Mobile</th><td>{data.mobileNumber}</td></tr>
              <tr><th>Address</th><td>{data.address}</td></tr>
              <tr><th>Designation</th><td>{data.designation}</td></tr>
              <tr><th>Salary</th><td>{data.salary}</td></tr>
              <tr><th>Organization</th><td>{data.organizationName} (#{data.organizationId})</td></tr>
            </tbody>
          </table>
        )}
      </div>
    </div>
  )
}
