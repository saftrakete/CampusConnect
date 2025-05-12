export class MessageEntity {
    constructor(
        content: string,
        messageId?: number
    ) {
        this.content = content;
        this.messageId = messageId;
    }

    public messageId?: number;
    public content: string; 
}
