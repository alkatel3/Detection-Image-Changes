export class Camera {
  port: number;
  name: string;
  src: string;

  constructor(port: number, name: string, src: string) {
    this.port = port;
    this.name = name;
    this.src = src;
  }
}