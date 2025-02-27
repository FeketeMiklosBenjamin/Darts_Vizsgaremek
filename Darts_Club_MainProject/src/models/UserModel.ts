export default interface UserModel {
    id?: string,
    username: string,
    password: string,
    emailAddress: string,
    role?: number,
    registerDate?: string,
    refreshToken?: string,
    lastLoginDate?: string,
    accessToken?: string
}