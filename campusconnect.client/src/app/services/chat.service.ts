import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MessageEntity } from '../entities/messageEntity';
import { baseApiRoute } from '../app-routing.module';
import { Observable } from 'rxjs';
import { messageDto } from '../entities/messageDto';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

    constructor(private httpClient: HttpClient) { }

    public postNewMessage(message: MessageEntity): Observable<MessageEntity> {
        return this.httpClient.post<MessageEntity>(baseApiRoute + "chat/post", message);
    }

    public sendMessageRequest(messageDto: MessageDto): Observable<MessageEntity> {
        return this.httpClient.post<MessageEntity>(baseApiRoute + 'chat/message', messageDto);
    }

    public createMessageEntity(
        content: string,
        messageId?: number
    ): MessageEntity {
        return new ChatEntity(content, messageId);
    }

    public createMessageDto(
        content: string,
    ): MessageDto {
        return new MessageDto(content);
    }
}
