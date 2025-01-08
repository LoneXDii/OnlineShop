import {Profile} from '../auth/profile.interface';

export interface PaginatedUsers {
  items: Profile[];
  currentPage: number;
  totalPages: number;
}
