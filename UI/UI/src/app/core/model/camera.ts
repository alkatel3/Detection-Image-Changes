export class Camera {
  port: number;
  width: number;
  height: number;
  name: string;
  src: string;

  constructor(port: number, name: string, src: string, width: number, height: number) {
    this.port = port;
    this.name = name;
    this.src = src;
    this.width = width;
    this.height = height;
  }
}