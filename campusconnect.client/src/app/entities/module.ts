export class Module {
  name: string = '';
  ID?: number;
  facultyID?: number;
  degreeID?: number;

constructor(n: string, ID?: number, facID?: number, degID?: number) {
  this.name = n;
  this.ID = ID;
  this.facultyID = facID;
  this.degreeID = degID;
}

}