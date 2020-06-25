export class LabelValue {
    constructor(l: string = "", v: number = 0) {
        this.label = l;
        this.value = v;
    }
    
    label: string;
    value: number;
}