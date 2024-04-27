export class Camera {
  id: string;
  name: string;
  src: string;

  constructor(id: string, name: string, src: string) {
    this.id = id;
    this.name = name;
    this.src = src;
  }
}