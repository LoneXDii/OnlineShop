export interface UserWithRoles {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  avatar: string | null;
  roles: string[];
}
