import { Component, OnInit, ViewChild } from '@angular/core';
import { ChatService } from '../services/chat.service';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { MessageEntity } from '../entities/message-entity';
import { MatCardModule } from '@angular/material/card';
import { UserService } from '../services/user.service';
import { AuthorizationService } from '../services/authorization.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss'
})
export class ChatComponent implements OnInit {
  constructor(private chatService: ChatService,
      private formBuilder: FormBuilder,
      private userService: UserService,
      private authorizationService: AuthorizationService
  ) {}

  public chatForm!: FormGroup;

    private readonly emptyString: string = '';

    private refreshIntervalId: any;


    public fetchedMessages: MessageEntity[] = [];

  public ngOnInit(): void {
      this.chatForm = this.formBuilder.group({
         chatMessage: [this.emptyString, Validators.required]
        }
      );


      
      this.fetchAllMessages(); // initial fetch
      this.refreshIntervalId = setInterval(() => {
          this.fetchAllMessages();
      }, 5000);
      
    }

    public ngOnDestroy(): void {
        // Clear the interval to avoid memory leaks
        if (this.refreshIntervalId) {
            clearInterval(this.refreshIntervalId);
        }
    }

  public chatClick(): void {
      if (!this.chatForm.valid.valueOf()) {
          console.log('unvalid form');
          return;
      }

      // TODO: use getUSerID function here
      /*
      const decodedToken = this.authorizationService.getDecodedToken();
      if (decodedToken) {
          console.log("User ID:", decodedToken);
      } else {
          console.log("No valid token found");
      }*/
      
      const userId = 9;
      //das sollte id bekommen vom eintrag im local stroage mit key "token"
      let chatEntity = this.chatService.createMessageEntity(
          this.chatForm.get('chatMessage')?.value,
          userId
      );
      
      this.chatService.postNewMessage(chatEntity).subscribe(
        response => {
          console.log(response);
        },
        error => {
 
        }
      );

      this.chatForm.reset();
  }

    public fetchAllMessages(): void {
        this.chatService.getAllMessages().subscribe({
            next: (messages) => {
                this.fetchedMessages = messages; // Alle Nachrichten direkt �bernehmen
                console.log('Alle Nachrichten geladen:', messages.length);
            },
            error: (error) => {
                console.error('Fehler beim Laden der Nachrichten:', error);
            }
        });
    }

    public clickFetchMessages(): void {
        this.fetchAllMessages();
    }

}
