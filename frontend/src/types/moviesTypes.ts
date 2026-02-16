import type { UserProfile } from "./auth";
import type { CommentResponse } from "./commentType";
import type { DirectorResponse } from "./directorsTypes";
import type { GenreResponse } from "./genreTypes";
import type { RatingResponse } from "./ratingType";

export interface CreateMovieRequest {
  title: string;
  timeLong: string;
  directorName: string;
  genreName: string;
  releaseDate: string;
}

export interface UpdateMovieRequest {
  title: string;
  timeLong: string;
  rating: number;
  numberOfVotes: number;
  directorName: string;
  genreName: string;
  releaseDate: string;
}

export interface MovieResponse {
  id: number;
  title: string;
  timeLong: string;
  director: DirectorResponse;
  releaseDate: string;
  genre: GenreResponse;
  ratings: RatingResponse[];
  numberOfVotes: number;
  createdAt: string;
  updatedAt: string;
  user: UserProfile;
  comments: CommentResponse[];
}

export interface UpdateMovieRatingRequest {
  username: string;
  rating: number;
}