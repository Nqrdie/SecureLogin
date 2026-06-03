import api from "./axios"

export async function login(email: string, password: string) {
  return api.post("/account/login", {
    email,
    password,
  })
}

export async function register(
  name: string,
  email: string,
  password: string
) {
  return api.post("/account/register", {
    name,
    email,
    password,
  })
}