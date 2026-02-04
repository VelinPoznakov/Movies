import { List } from "antd";
import MovieComponent from "../Components/MovieComponent";
import { useMovies } from "../Queries/Movies/movies";
import { useState } from "react";
import type { MovieResponse } from "../types/moviesTypes";
import MovieDetailsComponent from "../Components/MovieDetailsComponent";

function HomePage() {
  const { data, isLoading, error } = useMovies();

  const [isOpen, setIsOpen] = useState(false);
  const[modalMovie, setModalMovie] = useState<MovieResponse | null>(null);

  const handleOpenModal = (movie: MovieResponse) => {
    setIsOpen(true);
    setModalMovie(movie);
  }

  const handleCloseModal = () => {
    setIsOpen(false);
    setModalMovie(null);
  }

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error loading movies</div>;

  return (
    <>
      <List
        grid={{ gutter: 16, column: 4 }}
        dataSource={data ?? []}
        renderItem={(movie) => (
          <List.Item>
            <div
              role="button"
              style={{cursor: "pointer"}}
              onClick={() => handleOpenModal(movie)}
            >
              <MovieComponent movie={movie} />
            </div>
          </List.Item>
        )}
      />

      <MovieDetailsComponent open={isOpen} movie={modalMovie} onClose={handleCloseModal} />
    </>
  );
}

export default HomePage;
