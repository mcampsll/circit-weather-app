import { defineStore } from 'pinia'

interface GitHubUser {
  login: string
  name: string
  avatar_url: string
  bio: string
  public_repos: number
  followers: number
  following: number
  location: string
  company: string
  blog: string
  twitter_username: string
  created_at: string
  public_gists: number
}

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: JSON.parse(localStorage.getItem('gh_user') ?? 'null') as GitHubUser | null,
  }),
  getters: {
    isLoggedIn: (state) => state.user !== null,
  },
  actions: {
    setUser(user: GitHubUser) {
      this.user = user
      localStorage.setItem('gh_user', JSON.stringify(user))
    },
    logout() {
      this.user = null
      localStorage.removeItem('gh_user')
    },
  },
})
