export default interface UserModel {
    id?: string,
    username: string,
    password: string,
    emailAddress: string,
    role?: number,
    registerDate?: string,
    refreshToken?: string,
    refreshTokenExpiry?: string,
    lastLoginDate?: string,
    accessToken?: string
}