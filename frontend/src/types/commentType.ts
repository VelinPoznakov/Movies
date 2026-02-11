export interface CreateCommentsRequest {
  content: string;
}

export interface UpdateCommentRequest {
  content: string;
}

export interface CommentResponse {
  id: number;
  content: string;
  username: string;
  createdAt: string;
  updatedAt: string;
}