import {UserWithRoles} from './userWithRoles.interface';

export interface PaginatedUsers {
  items: UserWithRoles[];
  currentPage: number;
  totalPages: number;
}
