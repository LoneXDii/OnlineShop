export interface DecodedToken {
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress': string;
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role': string | null;
  Avatar: string | null;
  Id: string;
}
