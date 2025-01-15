import {inject, Injectable} from '@angular/core';
import {HubConnection, HubConnectionBuilder, HubConnectionState} from '@microsoft/signalr';
import {environment} from '../../../environments/environment';
import {Chat} from '../interfaces/signalR/chat.interface';
import {ChatMessage} from '../interfaces/signalR/chatMessage.interface';
import {AuthService} from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  authService = inject(AuthService);
  private  hubConnection: HubConnection;

  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/chat`, {
        withCredentials: true,
        accessTokenFactory: () => this.authService.getAccessToken
      })
      .withAutomaticReconnect()
      .build();
  }

  connect(){
    if (this.hubConnection.state === HubConnectionState.Disconnected) {
      return this.hubConnection
        .start()
        .then(() => console.log('Connection started'))
        .catch(err => console.log('Error while starting connection: ' + err));
    }

    return null;
  }

  getAllChats(){
    return this.hubConnection.invoke('GetAllChatsAsync');
  }

  receiveAllChats(callback: (chats: Chat[]) => void){
    this.hubConnection.on("ReceiveChats", (chats: Chat[]) => {
      callback(chats);
    });
  }

  getUserChats(){
    return this.hubConnection.invoke('GetUserChatsAsync');
  }

  receiveUserChats(callback: (chats: Chat[]) => void){
    this.hubConnection.on("ReceiveUserChats", (chats: Chat[]) => {
      callback(chats);
    });
  }

  getChatMessages(chatId: number){
    return this.hubConnection.invoke('GetChatMessagesAsync', chatId);
  }

  receiveChatMessages(callback: (messages: ChatMessage[]) => void){
    this.hubConnection.on("ReceiveChatMessages", (messages: ChatMessage[]) => {
      callback(messages);
    });
  }

  createChat(){
    return this.hubConnection.invoke('CreateChatAsync');
  }

  receiveNewChat(callback: (chat: Chat) => void){
    this.hubConnection.on("ReceiveNewChat", (chat: Chat) => {
      callback(chat);
    });
  }

  closeChat(chatId: number){
    return this.hubConnection.invoke('CloseChatAsync', chatId);
  }

  receiveClosedChat(callback: (chatId: number) => void){
    this.hubConnection.on("CloseChat", (chatId: number) => {
      callback(chatId);
    });
  }

  sendMessage(message: {text: string, chatId: number}) {
    return this.hubConnection.invoke('SendMessageAsync', message);
  }

  receiveMessage(callback: (message: ChatMessage) => void){
    this.hubConnection.on("ReceiveMessage", (message: ChatMessage) => {
      callback(message);
    });
  }

  getChatById(chatId: number){
    return this.hubConnection.invoke('GetChatByIdAsync', chatId);
  }

  receiveChat(callback: (chat: Chat) => void){
    this.hubConnection.on("ReceiveChat", (chat: Chat) => {
      callback(chat);
    });
  }
}
