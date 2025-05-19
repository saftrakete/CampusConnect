export class MessageEntity {
    constructor(
        content: string,
        userId: number,
        messageId?: number
        
    ) {
        this.content = content;
        this.userId = userId;
        this.messageId = messageId;
        
    }

    public content: string; 
    public userId: number;
    public messageId?: number;
}
