import {UserModel} from "./userModel";

export interface RoleDto {
    id: number,
    name: string,
    users: UserModel[]
}
