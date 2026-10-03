import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { api, clearAuth, getUser } from '../api'

const empty = {
  fullName: '',
  email: '',
  age: 25,
  mobileNumber: '',
  address: '',
  designation: '',
  salary: 0,
}

export default function OrgEmployees() {
  const nav = useNavigate()
  const user = getUser()
  const [list, setList] = useState([])
  const [form, setForm] = useState(empty)
  const [editId, setEditId] = useState(null)
  const [error, setError] = useState('')
  const [msg, setMsg] = useState('')

  function logout() {
    clearAuth()
    nav('/')
  }

  async function load() {
    setError('')
    try {
      setList(await api('/api/org/employees'))
    } catch (err) {
      setError(err.message)
    }
  }

  useEffect(() => { load() }, [])

  function setField(key, value) {
    setForm((f) => ({ ...f, [key]: value }))
  }

  function startEdit(emp) {
    setEditId(emp.id)
    setForm({
      fullName: emp.fullName,
      email: emp.email,
      age: emp.age,
      mobileNumber: emp.mobileNumber,
      address: emp.address,
      designation: emp.designation,
      salary: emp.salary,
    })
    setMsg('')
    setError('')
  }

  function cancelEdit() {
    setEditId(null)
    setForm(empty)
  }

  async function onSubmit(e) {
    e.preventDefault()
    setError('')
    setMsg('')
    const payload = {
      ...form,
      age: Number(form.age),
      salary: Number(form.salary),
    }
    try {
      if (editId) {
        await api(`/api/org/employees/${editId}`, {
          method: 'PUT',
          body: JSON.stringify(payload),
        })
        setMsg(`Updated employee #${editId}`)
      } else {
        await api('/api/org/employees', {
          method: 'POST',
          body: JSON.stringify(payload),
        })
        setMsg('Employee created.')
      }
      cancelEdit()
      load()
    } catch (err) {
      setError(err.message)
    }
  }

  async function remove(id) {
    if (!window.confirm(`Delete employee #${id}?`)) return
    setError('')
    try {
      await api(`/api/org/employees/${id}`, { method: 'DELETE' })
      setMsg(`Deleted #${id}`)
      if (editId === id) cancelEdit()
      load()
    } catch (err) {
      setError(err.message)
    }
  }

  return (
    <div className="wrap">
      <div className="topbar">
        <div>
          <strong>Org Employees</strong>{' '}
          <span className="muted">({user?.username})</span>
        </div>
        <button type="button" onClick={logout}>Logout</button>
      </div>

      {error && <p className="err">{error}</p>}
      {msg && <p className="ok">{msg}</p>}

      <div className="box">
        <h3 style={{ marginTop: 0 }}>{editId ? `Edit #${editId}` : 'Add employee'}</h3>
        <form onSubmit={onSubmit}>
          <div className="form-grid">
            <label>Full name</label>
            <input value={form.fullName} onChange={(e) => setField('fullName', e.target.value)} required />
            <label>Email</label>
            <input type="email" value={form.email} onChange={(e) => setField('email', e.target.value)} required />
            <label>Age</label>
            <input type="number" value={form.age} onChange={(e) => setField('age', e.target.value)} required />
            <label>Mobile</label>
            <input value={form.mobileNumber} onChange={(e) => setField('mobileNumber', e.target.value)} required />
            <label>Address</label>
            <input value={form.address} onChange={(e) => setField('address', e.target.value)} required />
            <label>Designation</label>
            <input value={form.designation} onChange={(e) => setField('designation', e.target.value)} required />
            <label>Salary</label>
            <input type="number" step="0.01" value={form.salary} onChange={(e) => setField('salary', e.target.value)} required />
            <div>
              <button type="submit">{editId ? 'Save' : 'Create'}</button>
              {editId && (
                <button type="button" onClick={cancelEdit} style={{ marginLeft: 8 }}>Cancel</button>
              )}
            </div>
          </div>
        </form>
      </div>

      <div className="box">
        <h3 style={{ marginTop: 0 }}>Employees</h3>
        <table>
          <thead>
            <tr>
              <th>Id</th>
              <th>Name</th>
              <th>Email</th>
              <th>Age</th>
              <th>Mobile</th>
              <th>Designation</th>
              <th>Salary</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {list.map((e) => (
              <tr key={e.id}>
                <td>{e.id}</td>
                <td>{e.fullName}</td>
                <td>{e.email}</td>
                <td>{e.age}</td>
                <td>{e.mobileNumber}</td>
                <td>{e.designation}</td>
                <td>{e.salary}</td>
                <td className="actions">
                  <button type="button" onClick={() => startEdit(e)}>edit</button>
                  <button type="button" onClick={() => remove(e.id)}>del</button>
                </td>
              </tr>
            ))}
            {list.length === 0 && (
              <tr><td colSpan={8} className="muted">no employees</td></tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  )
}
