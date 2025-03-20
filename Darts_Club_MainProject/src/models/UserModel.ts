export default interface UserModel {
    id: string,
    username: string,
    emailAddress: string,
    profilePictureUrl: string,
    role: number,
    accessToken: string,
    refreshToken: string,
    level: string
}