import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MessageEntity } from '../entities/message-entity';
import { baseApiRoute } from '../app-routing.module';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

    constructor(private httpClient: HttpClient) { }

    public postNewMessage(message: MessageEntity): Observable<MessageEntity> {
        return this.httpClient.post<MessageEntity>(baseApiRoute + "chat/post", message);
    }

    public createMessageEntity(
        content: string,
        userId: number
    ): MessageEntity {
        return new MessageEntity(content, userId);
    }

    public getMessageById(messageId: number): Observable<MessageEntity> {
        return this.httpClient.get<MessageEntity>(`${baseApiRoute}chat/get?messageId=${messageId}`);
    }

}
