# EmployeeAPI Frontend

Simple React (Vite) UI for EmployeeAPI.

## Run

1. API running on `https://localhost:7291`
2. In this folder:

```bash
npm install
npm run dev
```

Open http://localhost:5173

Vite proxies `/api` → `https://localhost:7291`.

## Pages

| Route | Role | What |
|-------|------|------|
| `/` | — | Login |
| `/admin` | Admin | Orgs, employees, create employee logins |
| `/org` | Organization | Employee CRUD |
| `/me` | Employee / Admin | Own profile |

Default admin: `admin` / `Admin@123`
