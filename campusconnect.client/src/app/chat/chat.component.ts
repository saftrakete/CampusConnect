import { Component, OnInit } from '@angular/core';
import { ChatService } from '../services/chat.service';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { MessageEntity } from '../entities/message-entity';
import { MatCardModule } from '@angular/material/card';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss'
})
export class ChatComponent implements OnInit {
  constructor(private chatService: ChatService,
      private formBuilder: FormBuilder,
      private userService: UserService
  ) {}

  public chatForm!: FormGroup;

  private readonly emptyString: string = '';

    public fetchedMessages: MessageEntity[] = [];

  public ngOnInit(): void {
      this.chatForm = this.formBuilder.group({
         chatMessage: [this.emptyString, Validators.required]
      }
    );
  }

  public chatClick(): void {
      if (!this.chatForm.valid.valueOf()) {
          console.log('unvalid form');
          return;
      }

      // TODO: use getUSerID function here
      
      
      var userId =Number( localStorage.getItem("token")) ;
      if (!userId) {
          console.error('User ID not found in local storage.');
          userId =-1;
          return;
      }
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

    public fetchMessages(): void {
        let index = 1;
        this.fetchedMessages = []; // reset list before new fetch

        const fetchNext = () => {
            this.chatService.getMessageById(index).subscribe({
                next: (response) => {
                    this.fetchedMessages.push(response);
                    index++;
                    fetchNext();
                },
                error: (error) => {
                    if (error.status === 404) {
                        console.log('No more messages found.');
                    } else {
                        console.error('Error fetching message:', error);
                    }
                }
            });
        };

        fetchNext();
    }



    public clickFetchMessages(): void {
        this.fetchMessages();
    }

}
