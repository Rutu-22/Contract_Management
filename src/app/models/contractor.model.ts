export enum Status {
  Active = 'Active',
  Inactive = 'Inactive',
  Pending = 'Pending'
}
export interface Contractor {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    mobile: string;
    gender: string;
    status: Status;
  }

  export interface ContractorResponse {
    Contractor: any;
    totalItems: number;
    data: Contractor[]; 
    msg: string;
    code: string;
  }

  