import { useEffect, useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { api, clearAuth, getUser } from '../api'

const emptyOrg = { name: '', username: '', password: '' }
const emptyAcct = { employeeId: '', username: '', password: '' }

export default function Admin() {
  const nav = useNavigate()
  const user = getUser()
  const [orgs, setOrgs] = useState([])
  const [emps, setEmps] = useState([])
  const [orgForm, setOrgForm] = useState(emptyOrg)
  const [acctForm, setAcctForm] = useState(emptyAcct)
  const [msg, setMsg] = useState('')
  const [error, setError] = useState('')

  function logout() {
    clearAuth()
    nav('/')
  }

  async function load() {
    setError('')
    try {
      const [o, e] = await Promise.all([
        api('/api/admin/organizations'),
        api('/api/admin/employees'),
      ])
      setOrgs(o)
      setEmps(e)
    } catch (err) {
      setError(err.message)
    }
  }

  useEffect(() => { load() }, [])

  async function createOrg(e) {
    e.preventDefault()
    setMsg('')
    setError('')
    try {
      await api('/api/admin/organizations', {
        method: 'POST',
        body: JSON.stringify(orgForm),
      })
      setMsg('Organization created.')
      setOrgForm(emptyOrg)
      load()
    } catch (err) {
      setError(err.message)
    }
  }

  async function createAccount(e) {
    e.preventDefault()
    setMsg('')
    setError('')
    try {
      await api('/api/admin/employee-accounts', {
        method: 'POST',
        body: JSON.stringify({
          ...acctForm,
          employeeId: Number(acctForm.employeeId),
        }),
      })
      setMsg('Employee login created.')
      setAcctForm(emptyAcct)
    } catch (err) {
      setError(err.message)
    }
  }

  return (
    <div className="wrap">
      <div className="topbar">
        <div>
          <strong>Admin</strong>{' '}
          <span className="muted">({user?.username})</span>
        </div>
        <div>
          <Link to="/me">/me</Link>{' '}
          <button type="button" onClick={logout}>Logout</button>
        </div>
      </div>

      {error && <p className="err">{error}</p>}
      {msg && <p className="ok">{msg}</p>}

      <div className="box">
        <h3 style={{ marginTop: 0 }}>Create organization</h3>
        <form onSubmit={createOrg}>
          <div className="form-grid">
            <label>Name</label>
            <input value={orgForm.name} onChange={(e) => setOrgForm({ ...orgForm, name: e.target.value })} required />
            <label>Username</label>
            <input value={orgForm.username} onChange={(e) => setOrgForm({ ...orgForm, username: e.target.value })} required />
            <label>Password</label>
            <input type="password" value={orgForm.password} onChange={(e) => setOrgForm({ ...orgForm, password: e.target.value })} required />
            <button type="submit">Create org</button>
          </div>
        </form>
      </div>

      <div className="box">
        <h3 style={{ marginTop: 0 }}>Organizations</h3>
        <table>
          <thead>
            <tr>
              <th>Id</th>
              <th>Name</th>
              <th>Employees</th>
            </tr>
          </thead>
          <tbody>
            {orgs.map((o) => (
              <tr key={o.id}>
                <td>{o.id}</td>
                <td>{o.name}</td>
                <td>{o.employeeCount}</td>
              </tr>
            ))}
            {orgs.length === 0 && (
              <tr><td colSpan={3} className="muted">none yet</td></tr>
            )}
          </tbody>
        </table>
      </div>

      <div className="box">
        <h3 style={{ marginTop: 0 }}>All employees</h3>
        <table>
          <thead>
            <tr>
              <th>Id</th>
              <th>Name</th>
              <th>Email</th>
              <th>Org</th>
              <th>Designation</th>
              <th>Salary</th>
            </tr>
          </thead>
          <tbody>
            {emps.map((e) => (
              <tr key={e.id}>
                <td>{e.id}</td>
                <td>{e.fullName}</td>
                <td>{e.email}</td>
                <td>{e.organizationName} ({e.organizationId})</td>
                <td>{e.designation}</td>
                <td>{e.salary}</td>
              </tr>
            ))}
            {emps.length === 0 && (
              <tr><td colSpan={6} className="muted">none yet</td></tr>
            )}
          </tbody>
        </table>
      </div>

      <div className="box">
        <h3 style={{ marginTop: 0 }}>Create employee login</h3>
        <form onSubmit={createAccount}>
          <div className="form-grid">
            <label>EmployeeId</label>
            <input type="number" value={acctForm.employeeId} onChange={(e) => setAcctForm({ ...acctForm, employeeId: e.target.value })} required />
            <label>Username</label>
            <input value={acctForm.username} onChange={(e) => setAcctForm({ ...acctForm, username: e.target.value })} required />
            <label>Password</label>
            <input type="password" value={acctForm.password} onChange={(e) => setAcctForm({ ...acctForm, password: e.target.value })} required />
            <button type="submit">Create account</button>
          </div>
        </form>
      </div>
    </div>
  )
}
