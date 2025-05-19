import { Component, OnInit } from '@angular/core';
import { ChatService } from '../services/chat.service';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss'
})
export class ChatComponent implements OnInit {
  constructor(private chatService: ChatService,
    private formBuilder: FormBuilder
  ) {}

  public chatForm!: FormGroup;

  private readonly emptyString: string = '';

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

      let chatEntity = this.chatService.createMessageEntity(
          this.chatForm.get('chatMessage')?.value,
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

    public fetchMessage(): void {
     var index = 0;
     var messages: MessageEntity[] = [];
     var condition = true;
     while (condition) {
         this.chatService.getMessageById(index).subscribe(
             response => {
                 console.log(response);
                 messages.push(response);
             },
             error => {
                 condition = false;
             }
         )
         index++;
    }
}

    public clickFetchMessages(): void {
        this.fetchMessage();
    }

}
