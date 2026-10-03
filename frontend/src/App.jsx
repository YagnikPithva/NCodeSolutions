import { Navigate, Route, Routes } from 'react-router-dom'
import { getToken, getUser } from './api'
import Login from './pages/Login'
import Admin from './pages/Admin'
import OrgEmployees from './pages/OrgEmployees'
import Me from './pages/Me'

function RequireAuth({ roles, children }) {
  const token = getToken()
  const user = getUser()
  if (!token || !user) return <Navigate to="/" replace />
  if (roles && !roles.includes(user.role)) {
    if (user.role === 'Admin') return <Navigate to="/admin" replace />
    if (user.role === 'Organization') return <Navigate to="/org" replace />
    return <Navigate to="/me" replace />
  }
  return children
}

export default function App() {
  return (
    <Routes>
      <Route path="/" element={<Login />} />
      <Route
        path="/admin"
        element={
          <RequireAuth roles={['Admin']}>
            <Admin />
          </RequireAuth>
        }
      />
      <Route
        path="/org"
        element={
          <RequireAuth roles={['Organization']}>
            <OrgEmployees />
          </RequireAuth>
        }
      />
      <Route
        path="/me"
        element={
          <RequireAuth roles={['Employee', 'Admin']}>
            <Me />
          </RequireAuth>
        }
      />
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  )
}
